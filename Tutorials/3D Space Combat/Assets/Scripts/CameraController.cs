using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float offset;
    public Transform player;

    public float moveSpeed;
    public float rotationSpeed;

    void LateUpdate()
    {
        Vector3 wantedPosition = player.position - (player.rotation * Vector3.forward) * offset;

        transform.position = Vector3.Lerp(transform.position, wantedPosition, moveSpeed);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, rotationSpeed);
    }
}
