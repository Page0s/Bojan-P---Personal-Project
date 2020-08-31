using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedModifier;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Camera camera;

    private float horizontal;
    private float vertical;
    private float camRayLength = 100f;
    private int floorMask;
    private Vector3 movement;
    private Rigidbody rigidbody;

    // private Animator animator;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        rigidbody = GetComponent<Rigidbody>();
        // animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Store the input axes.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move();

        // Turn the player to face the mouse cursor.
        Turning();
    }

    private void Turning()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;
            rigidbody.rotation = Quaternion.LookRotation(playerToMouse);
        }
    }

    void Move()
    {
        // Set the movement vector based on the axis input.
        movement.Set(horizontal, 0f, vertical);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement if Running.
        if (IsWalking() && Input.GetKey(KeyCode.LeftShift))
        {
            movement = movement.normalized * (speed * speedModifier) * Time.deltaTime;
            rigidbody.MovePosition(transform.position + movement);
        }
        else
        {
            // Move the player to it's current position plus the movement.
            rigidbody.MovePosition(transform.position + movement);
        }
    }

    // Returns a boolean that is true if either of the input axes is non-zero.
    private bool IsWalking()
    {
        return horizontal != 0f || vertical != 0f;
    }
}
