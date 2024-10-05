using UnityEngine;

[CreateAssetMenu(menuName = "Sound Manager", fileName = "Sound Manager")]
public class SoundManager : ScriptableObject
{
    public AudioSource audioSource;

    [Header("Sounds")] 
    public AudioClip rotationSound;
    public AudioClip dropSound;
    public AudioClip gameOverSound;

    private float _volumeChangeMultiplier = 0.15f;
    private float _pitchChangeMultiplier = 0.1f;

    public void PlaySound(AudioClip clip, float volume = 1f, Vector3 soundPos = default)
    {
        float randVolume = UnityEngine.Random.Range(volume - _volumeChangeMultiplier, volume + _volumeChangeMultiplier);
        float randPitch = UnityEngine.Random.Range(1 - _pitchChangeMultiplier, 1 + _pitchChangeMultiplier);

        audioSource.volume = randVolume;
        audioSource.pitch = randPitch;
        audioSource.PlayOneShot(clip);
    }
}