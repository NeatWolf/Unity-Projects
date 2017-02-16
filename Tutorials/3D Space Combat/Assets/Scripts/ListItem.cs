using UnityEngine;
using System.Collections;

public class ListItem : MonoBehaviour {

    [SerializeField]
    private string displayName;

    [Multiline]
    [SerializeField]
    private string description;

    [SerializeField]
    private Sprite sprite;
}
