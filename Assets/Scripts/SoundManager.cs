using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;
	
	public Sound[] musicSounds, sfxSounds;
	public AudioSource musicSource, sfxSource;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		CheckCurrentScene();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		CheckCurrentScene();
	}

	private void CheckCurrentScene()
	{
		if (SceneManager.GetActiveScene().name == "StartMenu")
		{
			PlayMusic("StartMenu", volume: 0.5f);
		}
		else if (SceneManager.GetActiveScene().name == "Game")
		{
			PlayMusic("Game", volume: 0.5f);	
		}
	}

	public void PlayMusic(string clipName, float volume = 1f)
	{
		Sound s = Array.Find(musicSounds, x => x.name == clipName);
		AudioClip randClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

		musicSource.Stop();
		musicSource.clip = randClip;
		musicSource.volume = volume;
		musicSource.Play();
	}

	public void PlaySfx(string clipName, float volume = 1f)
	{
		Sound s = Array.Find(sfxSounds, x => x.name == clipName);
		AudioClip randClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

		sfxSource.volume = volume;
		sfxSource.PlayOneShot(randClip);
	}

	public void ToggleMusic()
	{
		musicSource.mute = !musicSource.mute;
	}

	public void ToggleSFX()
	{
		sfxSource.mute = !sfxSource.mute;
	}

	public void MusicVolume(float volume)
	{
		musicSource.volume = volume;
	}

	public void SFXVolume(float volume)
	{
		sfxSource.volume = volume;
	}
}