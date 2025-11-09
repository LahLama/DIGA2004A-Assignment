using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference:
// https://www.daggerhartlab.com/unity-audio-and-sound-manager-singleton-script/
//https://www.daggerhartlab.com/unity-audio-and-sound-manager-singleton-script/
// This script provides a centralized sound manager using the Singleton pattern.
// It handles music, sound effects, random pitch variation, and looping audio clips.
public class SoundManager : MonoBehaviour
{
    // AudioSource components for different audio categories
    public AudioSource EffectsSource;     // For sound effects
    public AudioSource MusicSource;       // For background music
    public AudioSource MainMenuSource;    // For main menu music

    // Pitch variation range for randomized sound effects
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Singleton instance for global access
    public static SoundManager Instance = null;

    
    // Initialize the singleton instance
    private void Awake()
    {
        // If no instance exists, assign this one
        if (Instance == null)
        {
            Instance = this;
        }
        // Optional: destroy duplicate instances (commented out)
        /*else if (Instance != this)
        {
            Destroy(gameObject);
        }*/

        // Persist this GameObject across scene loads
       // DontDestroyOnLoad(gameObject);
    }

    // Play a single sound effect clip
    public void Play(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }

    // Play a music clip and stop main menu music
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
        MainMenuSource.Stop();
    }

    // Stop main menu music manually
    public void StopMainMenuMusic()
    {
        MainMenuSource.Stop();
    }

    // Play a random sound effect from an array with pitch variation
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }

    // Set volume for music sources
    public void SetMusicVolume(float volume)
    {
        MusicSource.volume = volume;
        if (MainMenuSource != null)
        {
            MainMenuSource.volume = volume;
        }
        else
        {
            Debug.LogWarning("MainMenuSource is not assigned in SoundManager.");
        }
    }

    // Set volume for sound effects
    public void SetEffectsVolume(float volume)
    {
        EffectsSource.volume = volume;
    }

    // Set volume for main menu music
    public void SetMainMenuVolume(float volume)
    {
        MainMenuSource.volume = volume;
    }

    // Dictionary to manage looping audio sources by name
    private Dictionary<string, AudioSource> _loopingSources = new Dictionary<string, AudioSource>();

    // Play a looping sound by name (loaded from Resources)
    public void PlayLooping(string clipName)
    {
        // If already playing, resume
        if (_loopingSources.ContainsKey(clipName))
        {
            if (!_loopingSources[clipName].isPlaying)
                _loopingSources[clipName].Play();
            return;
        }

        // Load clip from Resources folder
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (clip == null)
        {
            Debug.LogWarning($"Clip '{clipName}' not found in Resources.");
            return;
        }

        // Create and configure a new AudioSource for looping
        AudioSource loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.playOnAwake = false;
        loopSource.volume = EffectsSource.volume;
        loopSource.Play();

        // Store reference for future control
        _loopingSources.Add(clipName, loopSource);
    }

    // Stop a looping sound by name
    public void StopLooping(string clipName)
    {
        if (_loopingSources.ContainsKey(clipName))
        {
            _loopingSources[clipName].Stop();
        }
    }

    // Adjust volume of a looping sound
    public void SetLoopingVolume(string clipName, float volume)
    {
        if (_loopingSources.ContainsKey(clipName))
        {
            _loopingSources[clipName].volume = volume;
        }
    }
}