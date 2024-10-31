using System;
using System.Collections.Generic;
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
	public Dictionary<Tetromino, int> Statistics; 

	private void Awake() => ResetStatistics();

	private void OnEnable() => ResetData();

	public void ResetData()
	{
		score = 0;	
		linesCleared = 0;
		level = 1;
		ResetStatistics();
	}

	public void UpdateData(int lines)
	{
		UpdateScore(lines);	
		UpdateTopScore();
		UpdateLinesCleared(lines);
		UpdateLevel();
	}

	public void AddToStatistics(Tetromino tetromino) => Statistics[tetromino] += 1;
	
	private void UpdateScore(int lines)
	{
		int[] linePoints = { 0, oneLinePoints, twoLinePoints, threeLinePoints, fourLinePoints };
		score += linePoints[lines] * level;
	}

	private void UpdateTopScore()
	{
		if (topScore < score)
		{
			topScore = score;
		}	
	}
	
	private void UpdateLinesCleared(int lines) => linesCleared += lines;
	
	private void UpdateLevel() => level = Mathf.Max(1, linesCleared / linesForLevelUp + 1);

	private void ResetStatistics()
	{
		Statistics = new Dictionary<Tetromino, int>
		{
			{ Tetromino.T, 0 },	
			{ Tetromino.J, 0 },	
			{ Tetromino.Z, 0 },	
			{ Tetromino.O, 0 },	
			{ Tetromino.S, 0 },	
			{ Tetromino.L, 0 },	
			{ Tetromino.I, 0 }	
		};
	}
} 