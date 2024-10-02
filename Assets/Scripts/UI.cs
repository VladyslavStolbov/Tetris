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

}
