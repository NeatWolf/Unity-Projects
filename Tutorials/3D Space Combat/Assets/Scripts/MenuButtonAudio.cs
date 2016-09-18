using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class MenuButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    public AudioClip highlightClip;
    public AudioClip clickClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter sound");
        SoundManager.instance.PlaySingle(highlightClip, 0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click sound");
        SoundManager.instance.PlaySingle(clickClip, 0.5f);
    }
}
