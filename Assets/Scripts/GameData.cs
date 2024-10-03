using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")] public class GameData : ScriptableObject
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

	public Dictionary<Tetromino, int> Statistics; 

	private void Awake()
	{
		ResetStatistics();
	}

	private void OnEnable()
	{
		ResetData();
	}

	public void ResetData()
	{
		ClearScore();
		linesCleared = 0;
		level = 0;
		ResetStatistics();
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
				score += (oneLinePoints * (level != 0 ? level : 1)); // multiply on level, if level = 0 - multiply by 1
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

	public void AddToStatistics(Tetromino tetromino)
	{
		Statistics[tetromino] += 1;
	}

	private void ResetStatistics()
	{
		Statistics = new Dictionary<Tetromino, int>
		{
			{ Tetromino.T , 0},	
			{ Tetromino.J , 0},	
			{ Tetromino.Z , 0},	
			{ Tetromino.O , 0},	
			{ Tetromino.S , 0},	
			{ Tetromino.L , 0},	
			{ Tetromino.I , 0},	
		};
	}
} 
