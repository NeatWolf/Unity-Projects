using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float strafeSpeed;
    public float lookSpeed;

    private Rigidbody rb;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        // Look
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Quaternion rayDirection = Quaternion.LookRotation(ray.direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, rayDirection, lookSpeed);

        // Move
        rb.velocity = (transform.rotation * Vector3.forward) * moveSpeed * inputVertical;
        rb.velocity += (transform.rotation * Vector3.right) * strafeSpeed * inputHorizontal;
    }
}
