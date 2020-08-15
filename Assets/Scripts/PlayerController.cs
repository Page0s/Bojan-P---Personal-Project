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
    private Vector3 movement;

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

        if (Input.GetKey(KeyCode.LeftShift) && isMoving())
        {
            movement.x = horizontal;
            movement.z = vertical;

            transform.Translate(Vector3.ClampMagnitude(movement, 1f) * (speed * 2) * Time.deltaTime);
        }
        else
        {
            movement.x = horizontal;
            movement.z = vertical;

            transform.Translate(Vector3.ClampMagnitude(movement, 1f) * speed * Time.deltaTime);
        }

        transform.Rotate(Vector3.up, mouseHorizontal * mouseSensitivity * Time.deltaTime);
    }

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
