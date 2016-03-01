using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public float repeatFrequency = 0f;
    public bool used = false;

    void Start()
    {
        used = false;
    }
}
