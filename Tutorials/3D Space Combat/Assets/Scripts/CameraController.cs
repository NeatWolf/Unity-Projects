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
        // Set the position behind the player by a given offset
        Vector3 wantedPosition = player.position - player.forward * offset;

        // Remove z-axis rotation
        Quaternion rotationXY = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, 0f);

        // Set the position and rotation
        transform.position = Vector3.Lerp(transform.position, wantedPosition, moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationXY, rotationSpeed);
    }
}
