using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;


    private List<Sound> previouslyPlayingMusic = new List<Sound>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

    

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }

        // Initialize audio based on saved settings
        ApplyMusicSetting(HelperFunctions.GetMusic());
        ApplySoundSetting(HelperFunctions.GetSound());



        if (HelperFunctions.GetMusic())
        {
            previouslyPlayingMusic = LoadPreviouslyPlayingMusic();
            foreach (Sound s in previouslyPlayingMusic)
            {
                s.source.Play();
            }
            previouslyPlayingMusic.Clear();
            PlayerPrefs.DeleteKey(PreviouslyPlayingKey);
        }

    }

    public void Play(SoundType soundType)
    {
        Sound s = Array.Find(sounds, sound => sound.soundType == soundType);

        if (s == null)
        {
            Debug.LogWarning("Sound of type: " + soundType + " not found!");
            return;
        }

        if (s.source.loop)
        {
            if (s.shouldPlay && HelperFunctions.GetMusic())
            {
                s.source.Play();
            }
        }
        else
        {
            if (HelperFunctions.GetSound())
            {
                if (s.playRandomClip && s.randomClips != null && s.randomClips.Count > 0)
                {
                    PlayRandomClip(s);
                }
                else
                {
                    s.source.Play();
                }
            }
        }
    }

    private void PlayRandomClip(Sound sound)
    {
        int index = UnityEngine.Random.Range(0, sound.randomClips.Count);
        sound.source.clip = sound.randomClips[index];
        sound.source.Play();
    }

    public void Stop(SoundType soundType)
    {
        Sound s = Array.Find(sounds, sound => sound.soundType == soundType);

        if (s == null)
        {
            Debug.LogWarning("Sound of type: " + soundType + " not found!");
            return;
        }

        s.source.Stop();
    }


    public void ToggleMusic(bool isOn)
    {
        HelperFunctions.SetMusic(isOn);

        if (!isOn)
        {
            // Save currently playing looped sounds
            previouslyPlayingMusic.Clear();
            foreach (Sound s in sounds)
            {
                if (s.source.loop && s.source.isPlaying)
                {
                    previouslyPlayingMusic.Add(s);
                    s.source.Stop();
                }
            }

            SavePreviouslyPlayingMusic(previouslyPlayingMusic);
        }
        else
        {
            previouslyPlayingMusic = LoadPreviouslyPlayingMusic();
            foreach (Sound s in previouslyPlayingMusic)
            {
                s.source.Play();
            }
            previouslyPlayingMusic.Clear();
            PlayerPrefs.DeleteKey(PreviouslyPlayingKey); // Optional: Clean up saved data after restore
        }

        ApplyMusicSetting(isOn);
    }


    public void PlayStopMusic(bool isOn)
    {
        if (!isOn)
        {
            // Save currently playing looped sounds
            previouslyPlayingMusic.Clear();
            foreach (Sound s in sounds)
            {
                if (s.source.loop && s.source.isPlaying)
                {
                    previouslyPlayingMusic.Add(s);
                    s.source.Stop();
                }
            }
        }
        else
        {
            // Resume only those that were playing before
            foreach (Sound s in previouslyPlayingMusic)
            {
                s.source.Play();
            }
            previouslyPlayingMusic.Clear();
        }
    }   

    public void ToggleSound(bool isOn)
    {
        HelperFunctions.SetSound(isOn);
        ApplySoundSetting(isOn);
    }


    private void ApplyMusicSetting(bool isOn)
    {
        foreach (Sound s in sounds)
        {
            if (s.source.loop) // Music
            {
                s.shouldPlay = isOn; // Store setting without playing or stopping
            }
        }
    }


    private void ApplySoundSetting(bool isOn)
    {
        foreach (Sound s in sounds)
        {
            if (!s.source.loop) // SFX
            {
                s.source.mute = !isOn;
            }
        }
    }

    private const string PreviouslyPlayingKey = "PreviouslyPlayingMusic";

    private void SavePreviouslyPlayingMusic(List<Sound> playingSounds)
    {
        List<string> playingSoundNames = new List<string>();
        foreach (Sound s in playingSounds)
        {
            playingSoundNames.Add(s.soundType.ToString());
        }

        string serialized = string.Join(",", playingSoundNames);
        PlayerPrefs.SetString(PreviouslyPlayingKey, serialized);
        PlayerPrefs.Save();
    }

    private List<Sound> LoadPreviouslyPlayingMusic()
    {
        List<Sound> result = new List<Sound>();
        if (!PlayerPrefs.HasKey(PreviouslyPlayingKey)) return result;

        string serialized = PlayerPrefs.GetString(PreviouslyPlayingKey);
        string[] soundNames = serialized.Split(',');

        foreach (string name in soundNames)
        {
            if (Enum.TryParse(name, out SoundType type))
            {
                Sound s = Array.Find(sounds, snd => snd.soundType == type);
                if (s != null)
                    result.Add(s);
            }
        }

        return result;
    }

}


public enum SoundType
{
    MAINMENU,
    GP,
    BUTTONCLICK,
    SFX_CountDownAlarm,
    SFX_Explosion,
    JetPackLoop,
    SFX_StopJetPack,
    itemCollected,
    SFX_ItemsSold,
    SFX_Digging
}


