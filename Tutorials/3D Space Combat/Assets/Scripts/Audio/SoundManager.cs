using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    public int audioSourcePoolCount = 10;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private int audioSourcePoolUsedCount = 0;
    private int audioSourcePoolIndex = 0;

	void Awake ()
    {
	    if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    void Start()
    {
        createAudioSources(audioSourcePoolCount);
    }

    public void PlaySingle(AudioClip clip)
    {
        AudioSource audioSource = getAudioSource();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySingle(AudioClip clip, float volume)
    {
        AudioSource audioSource = getAudioSource();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        AudioSource source = getAudioSource();
        source.pitch = randomPitch;
        source.clip = clips[randomIndex];
        source.Play();
    }
	
    private void createAudioSources(int count)
    {
        while (count > 0)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSourcePool.Add(audioSource);
            count--;
        }
    }

    private AudioSource getAudioSource()
    {
        if (audioSourcePoolIndex < audioSourcePool.Count)
        {
            if (!audioSourcePool[audioSourcePoolIndex].isPlaying)
            {
                return audioSourcePool[audioSourcePoolIndex];
            }
            else
            {
                audioSourcePoolIndex++;
                return getAudioSource();
            }
        }
        else
        {
            audioSourcePoolIndex = 0;
            return getAudioSource();
        }
    }

    private void cleanPool()
    {
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                audioSourcePool.Remove(source);
                Destroy(source);
            }
        }
    }
}
