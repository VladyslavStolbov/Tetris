using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	[SerializeField] private GameData gameData;

	[Header("Menus")] 
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject optionsMenu;

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

	
	private TetrisInput tetrisInput;
	
	private void Awake()
	{
		CreateDict();

		tetrisInput = new TetrisInput();
		tetrisInput.UI.Cancel.performed += context => TogglePauseMenu();
	}

	private void OnEnable()
	{
		tetrisInput.Enable();
		UpdateUI();
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
		if (optionsMenu.activeSelf) return;
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		Time.timeScale = Time.timeScale == 1f ? 0 : 1;
	}

	public void ToggleOptionsMenu()
	{
		optionsMenu.SetActive(!optionsMenu.activeSelf);
		pauseMenu.SetActive(!pauseMenu.activeSelf);
	}
	
	public void UpdateUI()
	{
		scoreText.text = $"{gameData.score:000000}";
		topScoreText.text = $"{gameData.topScore:000000}";
		levelText.text = $"{gameData.level:00}"; 
		linesClearedText.text = $"{gameData.linesCleared:00}";
		UpdateStatisticsUI();
	} 

	private void UpdateStatisticsUI()
	{
		foreach (var entry in _tetrominoTextMapping)
		{
			entry.Value.text = $"{gameData.Statistics[entry.Key]:000}";
		}
	}
}