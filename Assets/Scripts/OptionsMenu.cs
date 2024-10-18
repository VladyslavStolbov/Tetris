using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	[SerializeField] private Toggle musicToggle;
	[SerializeField] private Toggle sfxToggle;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

	private void Awake()
	{
		SetupSoundPrefs();
		SoundManager.Instance.ApplyVolume();
	}
	
	public void SetupSoundPrefs()
	{
		musicToggle.isOn = PlayerPrefs.GetInt("MusicOn") != 0;
		sfxToggle.isOn = PlayerPrefs.GetInt("SFXOn") != 0;
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
	}

	public void SetMusic()
	{
        SoundManager.Instance.SetMusic(musicToggle.isOn);
	}    

	public void SetSFX()
	{
        SoundManager.Instance.SetSFX(sfxToggle.isOn);
	}

	public void ToggleMusic()
	{
		SoundManager.Instance.ToggleMusic();
	}

	public void ToggleSFX()
	{
		SoundManager.Instance.ToggleSFX();
	}

	public void MusicVolume()
	{
        SoundManager.Instance.MusicVolume(musicSlider.value);
	}

	public void SFXVolume()
	{
        SoundManager.Instance.SFXVolume(sfxSlider.value);
	}

	public void SaveVolume()
	{
		PlayerPrefs.SetInt("MusicOn", musicToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("SFXOn", sfxToggle.isOn ? 1 : 0);
		PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
		PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
		PlayerPrefs.Save();
	}
}
