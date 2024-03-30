using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem
{
    public AudioMixer audioMixer;

    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float effectVolume = 1f;

    public Action OnVolumeChanged;

    public AudioSource bgmAudioSource;
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        OnVolumeChanged?.Invoke();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        OnVolumeChanged?.Invoke();
    }

    public void SetEffectVolume(float volume)
    {
        effectVolume = volume;
        OnVolumeChanged?.Invoke();
    }

    public void SaveToJson()
    {
        var data = new AudioSystem
        {
            masterVolume = masterVolume,
            musicVolume = musicVolume,
            effectVolume = effectVolume
        };

        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("AudioSystemData", json);
    }

    public void LoadFromJson()
    {
        var json = PlayerPrefs.GetString("AudioSystemData");
        var data = JsonUtility.FromJson<AudioSystem>(json);

        masterVolume = data.masterVolume;
        musicVolume = data.musicVolume;
        effectVolume = data.effectVolume;
    }
}
