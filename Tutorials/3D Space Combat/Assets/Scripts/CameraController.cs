using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float offset;
    public Transform player;
    public Transform cam;
    public float moveSpeed;
    public float rotationSpeed;

    private bool isLocked = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
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
            }
        }
    }

    public void PerformDock()
    {
        //isLocked = true;
    }

    public void PerformUndock()
    {
        //anim.SetTrigger("Undock");
        //Invoke("UnlockController", 6f);
    }

    private void LockController()
    {

    }

    //private void UnlockController()
    //{
    //    transform.position = cam.transform.localPosition;
    //    transform.rotation = cam.transform.localRotation;
    //    cam.transform.localPosition = Vector3.zero;
    //    cam.transform.localRotation = Quaternion.identity;
    //    isLocked = false;
    //}
}
