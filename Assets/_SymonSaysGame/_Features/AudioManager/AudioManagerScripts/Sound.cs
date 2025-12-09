using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
 
    public AudioClip audioClip;


    [Tooltip("If enabled, a random clip from the list below will be played.")]
    public bool playRandomClip = false;

    [Tooltip("List of audio clips to randomly choose from (used only if playRandomClip is true).")]
    public List<AudioClip> randomClips;


    public bool loop = false;
    public bool playOnAwake = false;
    public bool shouldPlay = true;

    [Range(0, 1)]
    public float volume;

    [Range(0.1f, 3)]
    public float pitch;

    public SoundType soundType;

    [HideInInspector]
    public AudioSource source;
}
