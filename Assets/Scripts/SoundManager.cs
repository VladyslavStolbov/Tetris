using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Sound
{
	public string name;
	public AudioClip[] clips;
}

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

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		ApplyVolume();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		CheckCurrentScene();
	}

	private void CheckCurrentScene()
	{
		PlayMusic("Game");	
	}

	public void PlayMusic(string clipName, bool loop = true)
	{
		Sound s = Array.Find(musicSounds, x => x.name == clipName);
		AudioClip randClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

		musicSource.Stop();
		musicSource.clip = randClip;
		musicSource.loop = loop;
		musicSource.Play();
	}

	public void PlaySfx(string clipName)
	{
		Sound s = Array.Find(sfxSounds, x => x.name == clipName);
		AudioClip randClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

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

	public void SetMusic(bool isOn)
	{
		musicSource.mute = !isOn;
	}

	public void SetSFX(bool isOn)
	{
		sfxSource.mute = !isOn;
	}

	public void ApplyVolume()
	{
		SetMusic(PlayerPrefs.GetInt("MusicOn") != 0);
		SetSFX(PlayerPrefs.GetInt("SFXOn") != 0);
		MusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
		SFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
	}
}