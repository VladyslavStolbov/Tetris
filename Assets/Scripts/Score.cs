using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	[SerializeField] private Text scoreText;
	[SerializeField] private Text topScoreText;

	private int _score;
	private int _topScore;
	private int oneLinePoints = 40;
	private int twoLinePoints = 100;
	private int threeLinePoints = 300;
	private int fourLinePoints = 1200;
	
	public void UpdateScore()
	{
		_score += oneLinePoints;
		scoreText.text = $"{_score:000000}";
	}
}
