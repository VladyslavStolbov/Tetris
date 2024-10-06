using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;
	
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

	public void PlayMusic(string clipName)
	{
		Sound s = Array.Find(musicSounds, x => x.name == clipName);

		if (s == null)
		{
			Debug.Log("Music Not Found");
		}
		else
		{
			musicSource.clip = s.clip;
			musicSource.Play();
		}
	}

	public void PlaySfx(string clipName)
	{
		Sound s = Array.Find(sfxSounds, x => x.name == name);

		if (s == null)
		{
			Debug.Log("Sound Not Found");
		}
		else
		{
			sfxSource.PlayOneShot(s.clip);
		}
	}
}
