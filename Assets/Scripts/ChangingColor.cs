using System;
using UnityEngine;

public class ChangingColor : MonoBehaviour
{
	[SerializeField] private GameData gameData;
	[SerializeField] private Material material;
	
	private float _hue = 1f;
	private int _previousLevel = 1;
	
	private void Start() => material.SetFloat("_Hue", _hue);

	public void AppendColor()
	{
		if (gameData.level > 1 && gameData.level != _previousLevel)
		{
			if (_hue > 0)
			{
				_hue -= 0.1f;
				_hue *= -1;
			}
			else if (_hue < 0)
			{
				_hue *= -1;
			}

			material.SetFloat("_Hue", _hue);
			_previousLevel = gameData.level;
		}
	}
}
