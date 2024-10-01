using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
	public int score;
	public int topScore;
	public int oneLinePoints;
	public int twoLinePoints;
	public int threeLinePoints;
	public int fourLinePoints;
	public int level;
	public int linesCleared;

	private void OnEnable()
	{
		ClearScore();	
	}

	public void UpdateScore(int lines)
	{
		switch (lines)
		{
			case 1:
				score += oneLinePoints;
				break;
			case 2:
				score += twoLinePoints;
				break;
			case 3:
				score += threeLinePoints;
				break;
			case 4:
				score += fourLinePoints;
				break;
			default:
				break;
		}
	}

	public void UpdateTopScore()
	{
		if (topScore < score)
		{
			topScore = score;
		}	
	}

	public void ClearScore() => score = 0;
} 
