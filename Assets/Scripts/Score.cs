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
	
	public void UpdateScore(int lines)
	{
		switch (lines)
		{
			case 1:
				_score += oneLinePoints;
				break;
			case 2:
				_score += twoLinePoints;
				break;
			case 3:
				_score += threeLinePoints;
				break;
			case 4:
				_score += fourLinePoints;
				break;
			default:
				break;
		}
		
		scoreText.text = $"{_score:000000}";
	}
}
