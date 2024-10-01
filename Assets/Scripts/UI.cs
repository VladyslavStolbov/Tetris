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
	[SerializeField] private Text linesText;
	
	public void UpdateScoreText()
	{
		scoreText.text = $"{gameData.score:000000}";
	}
	
	public void UpdateTopScoreText()
	{
		topScoreText.text = $"{gameData.topScore:000000}";
	}

	public void ClearScoreText()
	{
		scoreText.text = $"{gameData.score:000000}";
	}

	public void AddLevel()
	{
		levelText.text = $"{gameData.level:00}"; 
	}
}
