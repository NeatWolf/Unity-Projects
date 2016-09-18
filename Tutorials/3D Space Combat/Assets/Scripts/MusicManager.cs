using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public enum MusicState
    {
        Default,
        Combat
    };

    public AudioClip defaultMusic;
    public AudioClip combatMusic;

    private MusicState _state;
    private AudioSource _audioSource1;
    private AudioSource _audioSource2;
    private bool _usingAudioSource1 = true;

	void Start ()
    {
        var sources = GetComponents<AudioSource>();
        _audioSource1 = sources[0];
        _audioSource2 = sources[1];
        _audioSource1.clip = defaultMusic;
        _audioSource1.Play();
	}

    void Update()
    {
        bool enemyNear = false;
        TargetableObject[] objects = GameObject.FindObjectsOfType(typeof(TargetableObject)) as TargetableObject[];

        foreach (TargetableObject obj in objects)
        {
            if (obj.allegiance == TargetableObject.Allegiance.Enemy)
            {
                if (Vector3.Distance(GameManager.playerTransform.position, obj.transform.position) < 500)
                {
                    // change to combat music
                    if (_state != MusicState.Combat)
                    {
                        Crossfade(combatMusic, 5f);
                        _state = MusicState.Combat;
                    }
                    enemyNear = true;
                    break;
                }
            }
        }

        if (!enemyNear && _state != MusicState.Default)
        {
            Crossfade(defaultMusic, 10f);
            _state = MusicState.Default;
        }
    }

    public void Crossfade(AudioClip newTrack, float fadeTime = 1f)
    {
        AudioSource newAudioSource;
        AudioSource oldAudioSource;
        if (_usingAudioSource1)
        {
            newAudioSource = _audioSource2;
            oldAudioSource = _audioSource1;
        }
        else
        {
            newAudioSource = _audioSource1;
            oldAudioSource = _audioSource2;
        }

        newAudioSource.clip = newTrack;
        newAudioSource.volume = 0f;
        newAudioSource.Play();
        StartCoroutine(PerformCrossfade(oldAudioSource, newAudioSource, fadeTime));
    }

    IEnumerator PerformCrossfade(AudioSource oldSource, AudioSource newSource, float fadeTime)
    {
        float t = 0f;

        while(t < fadeTime)
        {
            newSource.volume = Mathf.Lerp(0f, 0.1f, t / fadeTime);
            oldSource.volume = 0.1f - newSource.volume;

            t += Time.deltaTime;
            yield return null;
        }

        newSource.volume = 0.1f;
        oldSource.volume = 0f;
    }
}
