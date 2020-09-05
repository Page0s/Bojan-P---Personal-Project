using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Camera camera;

    private float horizontal;
    private float vertical;
    private float fireRate = 3;
    private float nextTimeToFire = 0f;
    private float camRayLength = 100f;
    private int floorMask;
    private Vector3 movement;
    private Rigidbody rigidbody;
    private ParticleSystem gunParticle;
    private GameManager gameManager;
    private PlayerStats playerStats;

    // private Animator animator;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        rigidbody = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gunParticle = GameObject.Find("Particle Gun").GetComponent<ParticleSystem>();
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

    private void Update()
    {
        // Fire gun left click
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            gunParticle.Play();
        }
    }

    private void Turning()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;
            rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerToMouse), rotationSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
        // Set the movement vector based on the axis input.
        movement.Set(horizontal, 0f, vertical);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * playerStats.MovementSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement if Running.
        if (IsWalking() && Input.GetKey(KeyCode.LeftShift))
        {
            movement = movement.normalized * (playerStats.MovementSpeed * playerStats.SpeedModifier) * Time.deltaTime;
            rigidbody.MovePosition(transform.position + movement);
        }
        else if(IsWalking())
        {
            // Move the player to it's current position plus the movement.
            rigidbody.MovePosition(transform.position + movement);
        }
        else
        {
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    // Returns a boolean that is true if either of the input axes is non-zero.
    private bool IsWalking()
    {
        return horizontal != 0f || vertical != 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If enemy layer collided with the object, damage the player by the amount
        if(collision.gameObject.layer == 11)
        {
            gameManager.DamagePlayer(collision, playerStats.Health);
        }
    }
}
