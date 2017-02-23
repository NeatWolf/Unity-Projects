using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGroup : MonoBehaviour {

    [SerializeField]
    private Text name, key;

    public void SetValues(string name, string key)
    {
        this.name.text = name;
        this.key.text = key;
    }
}
