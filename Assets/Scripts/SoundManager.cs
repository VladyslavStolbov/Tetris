using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;
	
	public Sound[] musicSounds, sfxSounds;
	public AudioSource musicSource, sfxSource;

	[SerializeField] private Toggle musicToggle;
	[SerializeField] private Toggle sfxToggle;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

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
		SetupSoundPrefs();
		CheckCurrentScene();
	}
	
	public void SetupSoundPrefs()
	{
		musicToggle.isOn = PlayerPrefs.GetInt("MusicOn") != 0;
		sfxToggle.isOn = PlayerPrefs.GetInt("SFXOn") != 0;
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		CheckCurrentScene();
	}

	private void CheckCurrentScene()
	{
		if (SceneManager.GetActiveScene().name == "StartMenu")
		{
			PlayMusic("StartMenu");
		}
		else if (SceneManager.GetActiveScene().name == "Game")
		{
			PlayMusic("Game");	
		}
	}

	public void PlayMusic(string clipName)
	{
		Sound s = Array.Find(musicSounds, x => x.name == clipName);
		AudioClip randClip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

		musicSource.Stop();
		musicSource.clip = randClip;
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

	public void MusicVolume()
	{
		musicSource.volume = musicSlider.value;
	}

	public void SFXVolume()
	{
		sfxSource.volume = sfxSlider.value;
	}

	public void SaveVolume()
	{
		PlayerPrefs.SetInt("MusicOn", musicToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("SFXOn", sfxToggle.isOn ? 1 : 0);
		PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
		PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
	}

}
