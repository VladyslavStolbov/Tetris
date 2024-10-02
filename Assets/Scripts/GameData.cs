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
	public int linesForLevelUp;

	private void OnEnable()
	{
		ResetData();
	}

	public void ResetData()
	{
		ClearScore();
		linesCleared = 0;
		level = 0;
	}

	public void UpdateData(int lines)
	{
		UpdateScore(lines);	
		UpdateTopScore();
		UpdateLinesCleared(lines);
		UpdateLevel();
	}

	public void UpdateScore(int lines)
	{
		switch (lines)
		{
			case 1:
				score += (oneLinePoints * (level != 0 ? level : 1));
				break;
			case 2:
				score += (twoLinePoints * (level != 0 ? level : 1));
				break;
			case 3:
				score += (threeLinePoints * (level != 0 ? level : 1));
				break;
			case 4:
				score += (fourLinePoints * (level != 0 ? level : 1));
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

	public void UpdateLinesCleared(int lines) => linesCleared += lines;
	
	private void UpdateLevel()
	{
		level = linesCleared / linesForLevelUp;
	}
} 
