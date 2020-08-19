using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetTransform;   // The position that that camera will be following.
    public float smoothingSpeed = 5f;   // The speed with which the camera will be following.

    private Vector3 offset;             // The initial offset from the target.

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - targetTransform.position;
    }

    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 amingPosition = targetTransform.position + offset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, amingPosition, smoothingSpeed * Time.deltaTime);
    }
}
