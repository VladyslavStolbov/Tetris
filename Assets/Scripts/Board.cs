using System.Collections;
using System.Collections.Generic;
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
    private RectInt bounds
    {
        get
        {
            Vector2Int position = new(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition = new(0, 8, 0);
    public Vector3Int previewPosition = new(10, 6, 0);
    public Vector2Int boardSize = new(10, 20);
    public GameData gameData;
    public UnityEvent<int> onClearLines;
    public UnityEvent onGameOver;
    public UnityEvent<Tetromino> onPieceSpawn;
    
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
            onPieceSpawn.Invoke(activePiece.data.tetromino); 
        }
        else
        {
            onGameOver.Invoke();
        }
        SetNextPiece();
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        foreach (Vector3Int piecePos in piece.cells)
        {
            Vector3Int tilePosition = piecePos + position;

            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
            if (!bounds.Contains((Vector2Int)tilePosition))
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

        for (int row = bounds.yMin; row < bounds.yMax; row++)
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
            onClearLines.Invoke(rowsToClear.Count);
            
            isClearing = false;
        }
    }

    private void MoveRowsDown()
    {
        int targetRow = GetTargetRow();
        for (int row = targetRow + 1; row < bounds.yMax; row++)
        {
            if (IsRowEmpty(row)) continue;
            for (int col = bounds.xMin; col < bounds.xMax; col++)
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

    private int GetTargetRow()
    {
        for (int row = bounds.yMin; row < bounds.yMax; row++)
        {
            if (IsRowEmpty(row))
                return row;
        }
        return bounds.yMin;
    }

    private bool IsRowEmpty(int row)
    {
        for (int col = bounds.xMin; col < bounds.xMax; col++)
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
        int center = (bounds.xMin + bounds.xMax) / 2;

        for (int i = 0; i <= bounds.xMax - bounds.xMin; i++)
        {
            int left = center - i;
            int right = center + i;

            if (left >= bounds.xMin)
            {
                Vector3Int leftPos = new(left, row, 0);
                tilemap.SetTile(leftPos, null);
            }
            
            if (right < bounds.xMax)
            {
                Vector3Int rightPos = new(right, row, 0);
                tilemap.SetTile(rightPos, null);
            }

            yield return new WaitForSeconds(0.05f);
        }
        
    }
    
    private bool IsLineFull(int row)
    {
        for (int col = bounds.xMin; col < bounds.xMax; col++)
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