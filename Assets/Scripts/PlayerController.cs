using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float mouseSensitivity = 100f;

    private float horizontal;
    private float vertical;
    private float mouseHorizontal;
    private float camRayLenght = 100f;
    private int floorMask;
    private Vector3 movement;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");

        // Move faster if player is running
        if (Input.GetKey(KeyCode.LeftShift) && isMoving())
        {
            movement.x = horizontal;
            movement.z = vertical;

            transform.Translate(Vector3.ClampMagnitude(movement, 1f) * (speed * 2) * Time.deltaTime);
        }
        // Move normal speed in scene
        else
        {
            movement.x = horizontal;
            movement.z = vertical;

            transform.Translate(Vector3.ClampMagnitude(movement, 1f) * speed * Time.deltaTime);
        }

        // Turning the palyer in Scene
        transform.Rotate(Vector3.up, mouseHorizontal * mouseSensitivity * Time.deltaTime);
    }

    // Check if palyer is inputing
    private bool isMoving()
    {
        if (horizontal > 0.1 || horizontal < -0.1 || vertical > 0.1 || vertical < -0.1)
        {
            return true;
        }
        else
            return false;
    }
}
