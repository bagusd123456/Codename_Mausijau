using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings_PanelView : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;

    private void OnEnable()
    {
        if (AudioManager.Instance != null)
        {
            AudioSettings audioSettings = AudioManager.Instance.LoadSettingsFromJSON();

            masterVolumeSlider.value = audioSettings.masterVolume;
            musicVolumeSlider.value = audioSettings.musicVolume;
            effectVolumeSlider.value = audioSettings.effectVolume;
        }
    }

    public void ChangeVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(masterVolumeSlider.value);
            AudioManager.Instance.SetMusicVolume(musicVolumeSlider.value);
            AudioManager.Instance.SetEffectVolume(effectVolumeSlider.value);
            AudioManager.Instance.SaveSettingsToJSON();
        }
    }
}
