using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
[Serializable]
public class AudioLibrary : ScriptableObject
{
    [Header("Music Library")]
    public AudioClip mainMenu_BGM;
    public AudioClip level1_BGM;
    public AudioClip level2_BGM;
    public AudioClip level3_BGM;

    //[Header("Sound Effect Library")]
    //public AudioClip gameStartClip;
}
