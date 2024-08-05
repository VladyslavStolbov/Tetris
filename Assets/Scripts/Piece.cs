using UnityEngine;

public class Piece : MonoBehaviour
{
	public Board board { get; private set; }
	public TetrominoData data { get; private set; }
	public Vector3Int[] cells { get; private set; }
	public Vector3Int position { get; private set; }
	
	public void Initialize(Board board, Vector3Int position, TetrominoData data)
	{
		this.board = board;
		this.position = position;
		this.data = data;

		if (cells == null)
		{
			cells = new Vector3Int[data.cells.Length];
		}

		for (int i = 0; i < data.cells.Length; i++)
		{
			cells[i] = (Vector3Int)data.cells[i];
		}
	}

	private void Update()
	{
		board.Clear(this);
		
		if (Input.GetKeyDown(KeyCode.A))	
		{
			Move(Vector2Int.left);	
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			Move(Vector2Int.right);
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			Move(Vector2Int.down);
		}

		if (Input.GetKeyDown("space"))
		{
			HardDrop();
		}
		
		board.Set(this);
	}

	private void HardDrop()
	{
		while (Move(Vector2Int.down))
		{
			continue;
		}
	}

	private bool Move(Vector2Int translation)
	{
		Vector3Int newPosition = position;
		newPosition.x += translation.x;
		newPosition.y += translation.y;

		bool valid = board.IsValidPosition(this, newPosition);

		if (valid)
		{
			position = newPosition;
		}

		return valid;
	}
}
