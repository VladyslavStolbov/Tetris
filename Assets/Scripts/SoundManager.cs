using System;
using UnityEngine;

public enum SoundType
{
	Click,
	Drop,
	GameOver,
	LineClear,
	Rotation,
	TetrisClear
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
	[SerializeField] private SoundList[] soundList;
	private static SoundManager _instance;
	private AudioSource _audioSource;

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public static void PlaySound(SoundType sound, float volume = 1)
	{
		AudioClip[] clips = _instance.soundList[(int)sound].Sounds;
		AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
		_instance._audioSource.PlayOneShot(randomClip, volume);
	}
	
#if UNITY_EDITOR
	private void OnEnable()
	{
		string[] names = Enum.GetNames(typeof(SoundType));
		Array.Resize(ref soundList, names.Length);
		for (int i = 0; i < soundList.Length; i++)
		{
			soundList[i].name = names[i];
		}
	}
#endif
}

[Serializable]
public struct SoundList
{
	public AudioClip[] Sounds { get => sounds; }
	[HideInInspector] public string name;
	[SerializeField] private AudioClip[] sounds;
}
