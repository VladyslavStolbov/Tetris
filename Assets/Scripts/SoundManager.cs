using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Sound Manager", fileName = "Sound Manager")]
public class SoundManager : ScriptableObject
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<SoundManager>("Sound Manager");
            }

            return _instance;
        }
    }
    
    public AudioSource soundObject;

    private float _volumeChangeMultiplier = 0.15f;
    private float _pitchChangeMultiplier = 0.1f;

    public void PlaySound(AudioClip clip, Vector3 soundPos, float volume)
    {
        float randVolume = UnityEngine.Random.Range(volume - _volumeChangeMultiplier, volume + _volumeChangeMultiplier);
        float randPitch = UnityEngine.Random.Range(1 - _pitchChangeMultiplier, 1 + _pitchChangeMultiplier);

        AudioSource a = Instantiate(Instance.soundObject, soundPos, quaternion.identity);

        a.clip = clip;
        a.volume = randVolume;
        a.pitch = randPitch;
        a.Play();
    }
}


