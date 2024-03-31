using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
[Serializable]
public class AudioLibrary : ScriptableObject
{
    [Header("Sound Library")]
    public AudioClip bgmInGameClip;
    public AudioClip bgmMainMenuClip;
    public AudioClip bgmCutscene_GoodEndingClip;
    public AudioClip bgmCutscene_BadEndingClip;

    [Header("Sound Effect")]
    public AudioClip gameStartClip;
    public AudioClip gameEndClip;
    public AudioClip confirmClickClip;
    public AudioClip declineClickClip;
    public AudioClip interactionCorrectClip;
    public AudioClip qteRollClip;
    public AudioClip swipeTransitionClip;
}
