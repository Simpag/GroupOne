using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

	public static AudioManager Instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

    private List<Sound> soundsPlaying;

	void Awake()
	{
        soundsPlaying = new List<Sound>();

		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

    private void Start()
    {
        Play("MainMenuMusic");
    }

    public void Play(string _sound)
	{
		Sound s = Array.Find(sounds, item => item.name == _sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + _sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
        soundsPlaying.Add(s);

        //Debug.Log("Playing " + _sound);
	}

    public void Stop(string _sound)
    {
        Sound s = Array.Find(sounds, item => item.name == _sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + _sound + " not found!");
            return;
        }

        s.source.Stop();
        soundsPlaying.Remove(s);

        //Debug.Log("Stopped " + _sound);
    }

    public bool isPlaying(string _sound)
    {
        //Debug.Log("Looking for " + _sound);

        Sound s = Array.Find(sounds, item => item.name == _sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + _sound + " not found!");
            return false;
        }

        if (soundsPlaying.Contains(s))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
