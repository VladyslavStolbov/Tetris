using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Piece : MonoBehaviour
{
	public Board board { get; private set; }
	public TetrominoData data { get; private set; }
	public Vector3Int[] cells { get; private set; }
	public Vector3Int position { get; private set; }
	public int rotationIndex { get; private set; }
	public TetrisInput tetrisInput { get; private set; }

	private float _stepDelay = 1f;
	private float _lockDelay = 0.3f;
	
	private float _stepTime;
	private float _lockTime;
	
	public void Initialize(Board board, Vector3Int position, TetrominoData data)
	{
		this.board = board;
		this.position = position;
		this.data = data;
		
		rotationIndex = 0;
		_stepTime = Time.time + _stepDelay;
		_lockTime = 0f;

		if (cells == null)
		{
			cells = new Vector3Int[data.cells.Length];
		}

		for (int i = 0; i < data.cells.Length; i++)
		{
			cells[i] = (Vector3Int)data.cells[i];
		}
	}

	private void Awake()
	{
		tetrisInput = new TetrisInput();

		ManageMovement();
		tetrisInput.Gameplay.RotateLeft.performed += context => Rotate(-1);
		tetrisInput.Gameplay.RotateRight.performed += context => Rotate(1); 
		tetrisInput.Gameplay.HardDrop.performed += context => HardDrop();
	}

	private void ManageMovement()
	{
		RegisterMovement(tetrisInput.Gameplay.MoveLeft, MoveLeft);
		RegisterMovement(tetrisInput.Gameplay.MoveRight, MoveRight);
		RegisterMovement(tetrisInput.Gameplay.MoveDown, MoveDown);
	}

	private void RegisterMovement(InputAction action, Action moveAction)
	{
		action.performed += context =>
		{
			if (context.interaction is HoldInteraction)
			{
				InvokeRepeating(moveAction.Method.Name,0f, 0.05f);
			}
			else
			{
				moveAction();
			}
		};

		action.canceled += context =>
		{
			CancelInvoke(moveAction.Method.Name);
		};
	}

	private void OnEnable()
	{
		tetrisInput.Enable();	
	}

	private void OnDisable()
	{
		tetrisInput.Disable();
	}

	private void Update()
	{
		board.Clear(this);

		_lockTime += Time.deltaTime;
		
		if (Time.time >= _stepTime)
		{
			Step();
		}
		
		board.Set(this);
	}

	private void Step()
	{
		_stepTime = Time.time + _stepDelay;

		Move(Vector2Int.down);

		if (_lockTime >= _lockDelay)
		{
			Lock();
		}
	}

	private void Lock()
	{
		board.Set(this);
		board.ClearLines();
		board.SpawnPiece();
		
		Debug.Log("Lock");
	}

	private void HardDrop()
	{
		board.Clear(this);
		
		Vector3Int dropPosition = position;

		while (board.IsValidPosition(this, dropPosition + Vector3Int.down))
		{
			dropPosition += Vector3Int.down;
		}

		position = dropPosition;
		board.Set(this);
		
		Debug.Log("HardDrop");
		Lock();
	}

	private bool Move(Vector2Int translation)
	{
		board.Clear(this);
		
		Vector3Int newPosition = position;
		newPosition.x += translation.x;
		newPosition.y += translation.y;

		bool valid = board.IsValidPosition(this, newPosition);

		if (valid)
		{
			position = newPosition;
			_lockTime = 0f;
		}
		return valid;
	}

	private void MoveLeft() => Move(Vector2Int.right);
	private void MoveRight() => Move(Vector2Int.left);
	private void MoveDown() => Move(Vector2Int.down);
	
	private void Rotate(int direction)
	{
		board.Clear(this);
		
		int originalRotation = rotationIndex;
		rotationIndex += Wrap(rotationIndex + direction, 0, 4);
		
		ApplyRotationMatrix(direction);

		if (!TestWallKicks(rotationIndex, direction))
		{
			rotationIndex = originalRotation;
			ApplyRotationMatrix(-direction);
		}
		
	}

	private void ApplyRotationMatrix(int direction)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			Vector3 cell = cells[i];
			int x, y;

			switch (data.tetromino)
			{
				case Tetromino.I:
				case Tetromino.O:
					cell.x -= 0.5f;
					cell.y -= 0.5f;
					x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
					y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
					break;
				
				default:
					x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
					y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
					break;
			}

			cells[i] = new Vector3Int(x, y, 0);
		}
		
	}
	
	private bool TestWallKicks(int rotationIndex, int rotationDirection)
	{
		int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

		for (int i = 0; i < data.wallKicks.GetLength(1); i++)
		{
			Vector2Int translation = data.wallKicks[wallKickIndex, i];

			if (Move(translation))
			{
				return true;
			}
		}

		return false;
	}

	private int GetWallKickIndex(int rotationIndex, int rotationDirection)
	{
		int wallKickIndex = rotationIndex * 2;

		if (rotationDirection < 0)
		{
			wallKickIndex--;
		}

		return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
	}
	
	private int Wrap(int input, int min, int max)
	{
		if (input < min)
		{
			return max - (min - input) % (max - min);
		}
		else
		{
			return min + (input - min) % (max - min);
		}
	}
}
