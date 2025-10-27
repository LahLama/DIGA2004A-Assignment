using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.daggerhartlab.com/unity-audio-and-sound-manager-singleton-script/
public class SoundManager : MonoBehaviour
{
	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;
	public AudioSource MainMenuSource;

	
	
	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Singleton instance.
	public static SoundManager Instance = null;
	
	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		/*else if (Instance != this)
		{
			Destroy(gameObject);
		}*/

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip)
	{
		EffectsSource.clip = clip;
		EffectsSource.Play();
		
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip)
	{
		MusicSource.clip = clip;
		MusicSource.Play();
		MainMenuSource.Stop();
		
	}

	public void StopMainMenuMusic()
	{
		
		MainMenuSource.Stop();
		
		
	}

	

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		EffectsSource.pitch = randomPitch;
		EffectsSource.clip = clips[randomIndex];
		EffectsSource.Play();
	}

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

public void SetEffectsVolume(float volume)
{
    EffectsSource.volume = volume;
}

public void SetMainMenuVolume(float volume)
{
    MainMenuSource.volume = volume;
}

private Dictionary<string, AudioSource> _loopingSources = new Dictionary<string, AudioSource>();

public void PlayLooping(string clipName)
{
    if (_loopingSources.ContainsKey(clipName))
    {
        if (!_loopingSources[clipName].isPlaying)
            _loopingSources[clipName].Play();
        return;
    }

    AudioClip clip = Resources.Load<AudioClip>(clipName);
    if (clip == null)
    {
        Debug.LogWarning($"Clip '{clipName}' not found in Resources.");
        return;
    }

    AudioSource loopSource = gameObject.AddComponent<AudioSource>();
    loopSource.clip = clip;
    loopSource.loop = true;
    loopSource.playOnAwake = false;
    loopSource.volume = EffectsSource.volume;
    loopSource.Play();

    _loopingSources.Add(clipName, loopSource);
}

public void StopLooping(string clipName)
{
    if (_loopingSources.ContainsKey(clipName))
    {
        _loopingSources[clipName].Stop();
    }
}
	
}

