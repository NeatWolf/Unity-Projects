using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float offset;
    public Transform player;

    public float moveSpeed;
    public float rotationSpeed;
    public float shakeAmplitude;
    public bool isShaking = false;

    private bool isLocked = false;

    void Update()
    {
        if(player != null)
        {
            if (!isLocked)
            {
                // Set the position behind the player by a given offset
                Vector3 wantedPosition = player.position - player.forward * offset;

                // Remove z-axis rotation
                Quaternion rotationXY = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, 0f);

                // Set the position and rotation
                transform.position = Vector3.Lerp(transform.position, wantedPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationXY, rotationSpeed * Time.deltaTime);

                if (isShaking)
                {
                    Vector3 initialPosition = transform.position;
                    transform.position = initialPosition + Random.insideUnitSphere * shakeAmplitude;
                }
            }
        }
    }

    public void PerformDock()
    {
        isLocked = true;
    }

    public void PerformUnDock()
    {
        isLocked = false;
    }
}
