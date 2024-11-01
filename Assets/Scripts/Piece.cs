using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Piece : MonoBehaviour
{
	public TetrominoData data { get; private set; }
	public Vector3Int[] cells { get; private set; }
	public Vector3Int position { get; private set; }

	private float _stepDelay;
	private float _lockDelay = 0.3f;
	
	private Board _board;
	private TetrisInput _tetrisInput;
	private int _rotationIndex;
	private float _stepTime;
	private float _lockTime;
	
	public void Initialize(Board board, Vector3Int position, TetrominoData data)
	{
		_board = board;
		this.position = position;
		this.data = data;

		SetStepDelay(board.gameData.level);
		_rotationIndex = 0;
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
		_tetrisInput = new TetrisInput();

		ManageMovement();
		_tetrisInput.Gameplay.RotateLeft.performed += _ => Rotate(-1);
		_tetrisInput.Gameplay.RotateRight.performed += _ => Rotate(1); 
		_tetrisInput.Gameplay.HardDrop.performed += _ => HardDrop();
	}

	private void OnEnable()
	{
		_tetrisInput.Enable();	
	}

	private void OnDisable()
	{
		_tetrisInput.Disable();
	}

	private void Update()
	{
		if (_board.isClearing) return;
		
		ManageGameplayInput();
		
		_board.Clear(this);

		_lockTime += Time.deltaTime;
		
		if (Time.time >= _stepTime)
		{
			Step();
		}
		
		_board.Set(this);
	}

	private void ManageGameplayInput()
	{
		if (Time.timeScale == 0)
		{
			_tetrisInput.Gameplay.Disable();
		}
		else
		{
			_tetrisInput.Gameplay.Enable();
		}
	}

	private void ManageMovement()
	{
		RegisterMovement(_tetrisInput.Gameplay.MoveLeft, MoveLeft);
		RegisterMovement(_tetrisInput.Gameplay.MoveRight, MoveRight);
		RegisterMovement(_tetrisInput.Gameplay.MoveDown, MoveDown);
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
			SoundManager.Instance.PlaySfx("Move");
		};

		action.canceled += _ =>
		{
			CancelInvoke(moveAction.Method.Name);
		};
	}

	public void ResetStepDelay()
	{ 
		_stepDelay = 0.8f;
	}
	
	private void SetStepDelay(int level)
	{
		switch (level)
		{
			case < 10:
				_stepDelay = 0.8f - level * 0.08f;
				break;
			case >= 10 and <= 18:
				_stepDelay = 0.1f;
				break;
			default:
				_stepDelay = 0.067f;
				break;
		}
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
		_board.Set(this);
		StartCoroutine(ClearAndLock());
	}

	private IEnumerator ClearAndLock()
	{
		yield return StartCoroutine(_board.ClearLines());
		_board.SpawnPiece();
	}

	private void HardDrop()
	{
		if (_board.isClearing) return;
		
		_board.Clear(this);
		
		Vector3Int dropPosition = position;

		while (_board.IsValidPosition(this, dropPosition + Vector3Int.down))
		{
			dropPosition += Vector3Int.down;
		}

		position = dropPosition;
		_board.Set(this);
		
		Lock();

		SoundManager.Instance.PlaySfx("Drop");
	}

	private bool Move(Vector2Int translation)
	{
		if (_board.isClearing) return false;
		
		_board.Clear(this);
		
		Vector3Int newPosition = position;
		newPosition.x += translation.x;
		newPosition.y += translation.y;

		bool valid = _board.IsValidPosition(this, newPosition);

		if (valid)
		{
			position = newPosition;
			_lockTime = 0f;
		}
		return valid;
	}

	private void MoveRight() => Move(Vector2Int.right);
	private void MoveLeft() => Move(Vector2Int.left);
	private void MoveDown() => Move(Vector2Int.down);
	
	private void Rotate(int direction)
	{
		_board.Clear(this);
		
		int originalRotation = _rotationIndex;
		_rotationIndex += Wrap(_rotationIndex + direction, 0, 4);
		
		ApplyRotationMatrix(direction);

		if (!TestWallKicks(_rotationIndex, direction))
		{
			_rotationIndex = originalRotation;
			ApplyRotationMatrix(-direction);
		}
		
		SoundManager.Instance.PlaySfx("Rotation");
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
					x = Mathf.CeilToInt(cell.x * Data.RotationMatrix[0] * direction + cell.y * Data.RotationMatrix[1] * direction);
					y = Mathf.CeilToInt(cell.x * Data.RotationMatrix[2] * direction + cell.y * Data.RotationMatrix[3] * direction);
					break;
				
				default:
					x = Mathf.RoundToInt(cell.x * Data.RotationMatrix[0] * direction + cell.y * Data.RotationMatrix[1] * direction);
					y = Mathf.RoundToInt(cell.x * Data.RotationMatrix[2] * direction + cell.y * Data.RotationMatrix[3] * direction);
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
