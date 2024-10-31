using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	[SerializeField] private GameData gameData;

	[Header("Menus")] 
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private OptionsMenu optionsMenu;
	[SerializeField] private GameObject gameOverMenu;
	[SerializeField] private Text finalScoreText;
	
	[Header("Score")]
	[SerializeField] private Text scoreText;
	[SerializeField] private Text topScoreText;

	[Header("Level")]	
	[SerializeField] private Text levelText;
	
	[Header("Lines")] 
	[SerializeField] private Text linesClearedText;

	[Header("Statistics")] 
	[SerializeField] private Text purpleTText;
	[SerializeField] private Text blueJText;
	[SerializeField] private Text redZText;
	[SerializeField] private Text yellowOText;
	[SerializeField] private Text greenSText;
	[SerializeField] private Text orangeLText;
	[SerializeField] private Text cyanIText;

	private Dictionary<Tetromino, Text> _tetrominoTextMapping;

	
	private TetrisInput _tetrisInput;
	
	private void Awake()
	{
		CreateDict();

		_tetrisInput = new TetrisInput();
		_tetrisInput.UI.Cancel.performed += _ => TogglePauseMenu();
	}

	private void OnEnable()
	{
		_tetrisInput.Enable();
		Update();
	}
		
	private void CreateDict()
	{
		_tetrominoTextMapping = new Dictionary<Tetromino, Text>
		{
			{ Tetromino.I, cyanIText },
			{ Tetromino.O, yellowOText },
			{ Tetromino.T, purpleTText },
			{ Tetromino.J, blueJText },
			{ Tetromino.L, orangeLText },
			{ Tetromino.S, greenSText },
			{ Tetromino.Z, redZText }
		};
	}

	public void TogglePauseMenu()
	{
		if (optionsMenu.gameObject.activeSelf) return;
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		Time.timeScale = Time.timeScale == 1 ? 0 : 1;
	}

	public void ToggleOptionsMenu()
	{
		optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
		pauseMenu.SetActive(!pauseMenu.activeSelf);
	}
	
	public void ToggleGameOverMenu()
	{
		gameOverMenu.SetActive(!gameOverMenu.activeSelf);
		finalScoreText.text = gameData.score.ToString();
		SoundManager.Instance.PlaySfx("GameOver");
		Time.timeScale = Time.timeScale == 1 ? 0 : 1;
	}
	
	public void Update()
	{
		scoreText.text = $"{gameData.score:000000}";
		topScoreText.text = $"{gameData.topScore:000000}";
		levelText.text = $"{gameData.level:00}"; 
		linesClearedText.text = $"{gameData.linesCleared:00}";
		UpdateStatistics();
	} 

	private void UpdateStatistics()
	{
		foreach (var entry in _tetrominoTextMapping)
		{
			entry.Value.text = $"{gameData.Statistics[entry.Key]:000}";
		}
	}
}