using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Piece nextPiece { get; private set; }
    public bool isClearing { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition = new(0, 8, 0);
    public Vector3Int previewPosition = new(10, 6, 0);
    public Vector2Int boardSize = new(10, 20);
    public GameData gameData;
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    public UnityEvent<int> OnClearLines;
    public UnityEvent OnGameOver;
    public UnityEvent<Tetromino> OnPieceSpawn;
    
    private List<int> _bag = new(); 

    private void Awake()
    {
        Time.timeScale = 1;
        tilemap = GetComponentInChildren<Tilemap>();
        nextPiece = gameObject.AddComponent<Piece>();
        nextPiece.enabled = false;

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    { 
        activePiece = GetComponentInChildren<Piece>(); // Need to be there to fix issue with finding first reference
    
        FillBag();
        SetNextPiece();
        SpawnPiece();
    }

    private void FillBag()
    {
        HashSet<int> temp = new();
        int capacity = 7;
        while (temp.Count != capacity)
        {
            int random = Random.Range(0, tetrominoes.Length);
            temp.Add(random);
        }

        _bag = temp.ToList();
    }

    private void SetNextPiece()
    {
        if (nextPiece.cells != null) {
            Clear(nextPiece);
        }

        if (_bag.Count == 0)
        {
            FillBag();
        }
        
        int random = Random.Range(0, _bag.Count);
        TetrominoData data = tetrominoes[_bag[random]];
        _bag.RemoveAt(random);
        
        nextPiece.Initialize(this, previewPosition, data);

        Set(nextPiece);
    }

    public void SpawnPiece()
    {
        if (isClearing)
        {
            StartCoroutine(SpawnPieceAfterClearing());
        }
        else
        {
            SpawnPieceImmediate();
        }
    }

    private IEnumerator SpawnPieceAfterClearing()
    {
        while (isClearing)
        {
            yield return null;
        }
        SpawnPieceImmediate();
    }

    private void SpawnPieceImmediate()
    {
        activePiece.Initialize(this, spawnPosition, nextPiece.data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
            OnPieceSpawn.Invoke(activePiece.data.tetromino); 
        }
        else
        {
            OnGameOver.Invoke();
        }

        SetNextPiece();
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
            if (!Bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public IEnumerator ClearLines()
    {
        List<int> rowsToClear = new();

        for (int row = Bounds.yMin; row < Bounds.yMax; row++)
        {
            if (IsLineFull(row))
            {
                rowsToClear.Add(row);
            }
        }

        if (rowsToClear.Count > 0)
        {
            isClearing = true;
            
            foreach (int row in rowsToClear)
            {
                StartCoroutine(ClearLine(row));
            }

            SoundManager.Instance.PlaySfx("ClearLine");
            yield return new WaitForSeconds(0.5f);
            MoveRowsDown();
            OnClearLines.Invoke(rowsToClear.Count);
            
            isClearing = false;
        }
    }

    private void MoveRowsDown()
    {
        int targetRow = Bounds.yMin;
        for (int row = Bounds.yMin; row < Bounds.yMax; row++)
        {
            if (IsRowEmpty(row) || targetRow == row) continue;
            for (int col = Bounds.xMin; col < Bounds.xMax; col++)
            {
                Vector3Int sourcePosition = new(col, row, 0);
                TileBase tile = tilemap.GetTile(sourcePosition);

                Vector3Int targetPosition = new(col, targetRow, 0);
                tilemap.SetTile(targetPosition, tile);
                tilemap.SetTile(sourcePosition, null);
            }
            targetRow++;
        }
    }

    private bool IsRowEmpty(int row)
    {
        for (int col = Bounds.xMin; col < Bounds.xMax; col++)
        {
            Vector3Int position = new(col, row, 0);
        
            if (tilemap.HasTile(position))
            {
                return false; 
            }
        }
        return true;
    }

    private IEnumerator ClearLine(int row)
    {
        int center = (Bounds.xMin + Bounds.xMax) / 2;

        for (int i = 0; i <= Bounds.xMax - Bounds.xMin; i++)
        {
            int left = center - i;
            int right = center + i;

            if (left >= Bounds.xMin)
            {
                Vector3Int leftPos = new(left, row, 0);
                tilemap.SetTile(leftPos, null);
            }
            
            if (right < Bounds.xMax)
            {
                Vector3Int rightPos = new(right, row, 0);
                tilemap.SetTile(rightPos, null);
            }

            yield return new WaitForSeconds(0.05f);
        }
        
    }
    
    private bool IsLineFull(int row)
    {
        for (int col = Bounds.xMin; col < Bounds.xMax; col++)
        {
            Vector3Int position = new(col, row, 0);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }
}