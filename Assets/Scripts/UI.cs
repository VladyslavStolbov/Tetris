using System;
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
	
	private void OnEnable()
	{
		UpdateUI();
	}

	public void UpdateUI()
	{
		scoreText.text = $"{gameData.score:000000}";
		topScoreText.text = $"{gameData.topScore:000000}";
		levelText.text = $"{gameData.level:00}"; 
		linesClearedText.text = $"{gameData.linesCleared:00}";
	} 

	public void ClearScoreText()
	{
		scoreText.text = $"{gameData.score:000000}";
	}

	public void UpdateStatisticsUI(Tetromino tetromino)
	{
		switch (tetromino)
		{
			case Tetromino.I:
				CyanIText.text = $"{gameData.Statistics[Tetromino.I]:000}";	
				break;
			case Tetromino.O:
				YellowOText.text = $"{gameData.Statistics[Tetromino.O]:000}";	
				break;
			case Tetromino.T:
				PurpleTText.text = $"{gameData.Statistics[Tetromino.T]:000}";	
				break;
			case Tetromino.J:
				BlueJText.text = $"{gameData.Statistics[Tetromino.J]:000}";	
				break;
			case Tetromino.L:
				OrangeLText.text = $"{gameData.Statistics[Tetromino.L]:000}";	
				break;
			case Tetromino.S:
				GreenSText.text = $"{gameData.Statistics[Tetromino.S]:000}";	
				break;
			case Tetromino.Z:
				RedZText.text = $"{gameData.Statistics[Tetromino.Z]:000}";	
				break;
		}
	}

	public void ResetStatisticsUI()
	{
		CyanIText.text = "000"; 
		YellowOText.text = "000"; 
		PurpleTText.text = "000"; 
		BlueJText.text = "000"; 
		OrangeLText.text = "000"; 
		GreenSText.text = "000"; 
		RedZText.text = "000"; 
	}
}
