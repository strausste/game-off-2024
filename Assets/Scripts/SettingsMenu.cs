using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        sfxSlider.value = AudioManager.instance.GetSfxVolume();
        musicSlider.value = AudioManager.instance.GetMusicVolume();
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }
    
    public void SetSfxVolume(float volume)
    {
        AudioManager.instance.SetSfxVolume(volume);
    }
}
