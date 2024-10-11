using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	[SerializeField] private GameData gameData;

	[Header("Score")]
	[SerializeField] private Text scoreText;
	[SerializeField] private Text topScoreText;

	[Header("Level")]	
	[SerializeField] private Text levelText;
	
	[Header("Lines")] 
	[SerializeField] private Text linesClearedText;

	[Header("Statistics")] 
	[SerializeField] private Text PurpleTText;
	[SerializeField] private Text BlueJText;
	[SerializeField] private Text RedZText;
	[SerializeField] private Text YellowOText;
	[SerializeField] private Text GreenSText;
	[SerializeField] private Text OrangeLText;
	[SerializeField] private Text CyanIText;

	private Dictionary<Tetromino, Text> _tetrominoTextMapping;

	[Header("Menus")] 
	[SerializeField] private GameObject pauseMenu;
	
	private TetrisInput tetrisInput;
	
	private void Awake()
	{
		CreateDict();

		tetrisInput = new TetrisInput();
		tetrisInput.UI.Cancel.performed += context => TurnPauseMenu();
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
			{ Tetromino.I, CyanIText },
			{ Tetromino.O, YellowOText },
			{ Tetromino.T, PurpleTText },
			{ Tetromino.J, BlueJText },
			{ Tetromino.L, OrangeLText },
			{ Tetromino.S, GreenSText },
			{ Tetromino.Z, RedZText }
		};
	}

	public void TurnPauseMenu()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);

		Time.timeScale = Time.timeScale == 1f ? 0 : 1;
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
