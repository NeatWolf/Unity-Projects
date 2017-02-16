using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float repeatFrequency = 0f;
    [SerializeField]
    private bool used = false;

    public bool Used
    {
        get { return used; }
        set { used = value; }
    }

    void Start()
    {
        used = false;
    }
}
