using System;
using UnityEngine;

public enum SoundType
{
	FX,
	Music
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
	private static SoundManager instance;
	private AudioSource audioSource;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public static void PlaySound(SoundType sound, float volume = 1)
	{
		
	}
}
