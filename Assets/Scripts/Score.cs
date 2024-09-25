using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	[SerializeField] private Text scoreText;
	[SerializeField] private Text topScoreText;

	private int _score;
	private int _topScore;
	
	private void Update()
	{
		_score += 1;
		scoreText.text = $"{_score:000000}";
	}
}
