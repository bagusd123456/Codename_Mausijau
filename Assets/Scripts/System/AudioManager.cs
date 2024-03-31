using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public enum audioCategory { Master = 0, Music = 1, Effect = 2}

    public const string masterVolumeData = "masterVolume";
    public const string musicVolumeData = "musicVolume";
    public const string effectVolumeData = "effectVolume";
    public static AudioManager Instance { get; private set; }
    public static Action<AudioClip> OnPlaySFX;

    [Header("Audio Properties")]
    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioSource effectSource;

    [Header("Audio Assets")]
    private AudioClip[] audioTarget;

    public AudioSettings audioSetting;
    public AudioLibrary audioLibrary;
    private void Awake()
    {
        audioTarget = Resources.LoadAll<AudioClip>("Uwu");
        LoadSettingsFromJSON();
        AssignSettingsFromData();
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        LoadSettingsFromJSON();
        AssignSettingsFromData();

        OnPlaySFX += PlaySoundEffect;
        SceneManager.sceneLoaded += OnSceneLoadedHandler;
    }

    private void OnDisable()
    {
        OnPlaySFX -= PlaySoundEffect;
        SceneManager.sceneLoaded -= OnSceneLoadedHandler;
    }

    private void OnSceneLoadedHandler(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 0)
        {
            ChangeMusic(audioLibrary.mainMenu_BGM);
        }
        else if (arg0.buildIndex == 1)
        {
            ChangeMusic(audioLibrary.level1_BGM);
        }
        else if (arg0.buildIndex == 2)
        {
            ChangeMusic(audioLibrary.level2_BGM);
        }
        else if (arg0.buildIndex == 3)
        {
            ChangeMusic(audioLibrary.level3_BGM);
        }
    }

    public void ChangeMusic(AudioClip clip)
    {
        //Stop the music source from playing
        musicSource.Stop();
        //Change the music clip
        musicSource.clip = clip;
        //Play the music clip
        musicSource.Play();
    }

    /// <summary>
    /// Set the master volume of the game.
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", value);
    }

    /// <summary>
    /// Set the music volume of the game.
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
    }
    /// <summary>
    /// Set the SFX volume of the game.
    /// </summary>
    /// <param name="volume"></param>
    public void SetEffectVolume(float value)
    {
        audioMixer.SetFloat("EffectVolume", value);
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSoundEffect()
    {
        effectSource.mute = !effectSource.mute;
    }

    /// <summary>
    /// Play the SFX clip.
    /// </summary>
    /// <param name="sfxClip"></param>
    public void PlaySoundEffect(AudioClip sfxClip)
    {
        if(!effectSource.isPlaying)
            effectSource.PlayOneShot(sfxClip);
        else
        {
            effectSource.Stop();
            effectSource.PlayOneShot(sfxClip);
        }
    }

    public void PlaySoundEffectOneShot(AudioClip sfxClip)
    {
        effectSource.PlayOneShot(sfxClip);
    }

    public float GetMasterVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float value);
        return value;
        //return audioMixer.GetFloat("masterVolume", out float value) ? value : 0;
    }

    public float GetMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float value);
        return value;
    }

    public float GetEffectVolume()
    {
        audioMixer.GetFloat("EffectVolume", out float value);
        return value;
    }

    //Play BGM from the list of BGMs
    public void PlayBGM(AudioClip targetAudio)
    {
        musicSource.Stop();
        musicSource.clip = targetAudio;
        musicSource.Play();
    }

    public void SaveSettingsToJSON()
    {
        var data = new AudioSettings
        {
            masterVolume = GetMasterVolume(),
            musicVolume = GetMusicVolume(),
            effectVolume = GetEffectVolume()
        };

        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("AudioSettings", json);
    }

    public AudioSettings LoadSettingsFromJSON()
    {
        var defaultData = new AudioSettings()
        {
            masterVolume = 0.5f,
            musicVolume = 0.5f,
            effectVolume = 0.5f
        };

        string defaultDataJSON = JsonUtility.ToJson(defaultData, true);
        var data = JsonUtility.FromJson<AudioSettings>(PlayerPrefs.GetString("AudioSettings", defaultDataJSON));

        return data;
    }

    [ContextMenu("AssignSettingsFromData")]
    public void AssignSettingsFromData()
    {
        var data = LoadSettingsFromJSON();
        SetMasterVolume(data.masterVolume);
        SetMusicVolume(data.masterVolume);
        SetEffectVolume(data.effectVolume);
    }

    /// <summary>
    /// I don't know what this is for.
    /// </summary>
    public void UwUTrigger()
    {
        AudioManager.OnPlaySFX?.Invoke(audioTarget[Random.Range(0, audioTarget.Length)]);
    }
}

public class AudioSettings
{
    public float masterVolume;
    public float musicVolume;
    public float effectVolume;
}