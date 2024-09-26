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

    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition = new(0, 8, 0);
    public Vector3Int previewPosition = new(10, 6, 0);
    public Vector2Int boardSize = new(10, 20);
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
    
    private List<int> _bag = new (); 


    private void Awake()
    {
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
        
        int random = Random.Range(0, _bag.Count );
        TetrominoData data = tetrominoes[_bag[random]];
        _bag.RemoveAt(random);
        
        nextPiece.Initialize(this, previewPosition, data);

        Set(nextPiece);
    }

    public void SpawnPiece()
    {
        activePiece.Initialize(this, spawnPosition, nextPiece.data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            OnGameOver.Invoke();
        }

        SetNextPiece();
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

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (tilemap.HasTile(tilePosition))
            {
                Debug.Log("Has tile");
                return false;
            }

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                Debug.Log($"Position {tilePosition} is out of bounds!");
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;
        int linesAmount = 0;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                linesAmount += 1;
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
        
        OnClearLines.Invoke(linesAmount);
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

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

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}
