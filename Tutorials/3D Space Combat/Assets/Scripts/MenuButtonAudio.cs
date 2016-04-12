using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class MenuButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    public AudioClip highlightClip;
    public AudioClip clickClip;

    private AudioSource highlightSource;
    private AudioSource clickSource;

	void Awake ()
    {
        highlightSource = gameObject.AddComponent<AudioSource>();
        highlightSource.playOnAwake = false;
        highlightSource.clip = highlightClip;
        clickSource = gameObject.AddComponent<AudioSource>();
        clickSource.playOnAwake = false;
        clickSource.clip = clickClip;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter sound");
        highlightSource.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click sound");
        clickSource.Play();
    }
}
