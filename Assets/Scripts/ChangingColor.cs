using System;
using UnityEngine;

public class ChangingColor : MonoBehaviour
{
	[SerializeField] private GameData gameData;
	[SerializeField] private Material material;
	[SerializeField] private float hue;

	private void Update()
	{
		material.SetFloat("_Hue", hue);
	}
}
