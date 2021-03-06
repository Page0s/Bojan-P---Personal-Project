using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float FireRate { get => fireRate; set => fireRate = value; }

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Camera camera;
    [SerializeField] private AudioClip gunShoot;

    private float horizontal;
    private float vertical;
    private float fireRate = 5;
    private float nextTimeToFire = 0f;
    private float camRayLength = 100f;
    private int floorMask;
    private Vector3 movement;
    private Rigidbody rigidbody;
    private ParticleSystem gunParticle;
    private SoundManager soundManager;
    private PlayerStats playerStats;
    private AudioSource audioSource;
    private FootStept footStept;
    private bool enemyBite;
    private bool oneBite;
    private float nextTimeToBite = 0f;
    private float biteRate = 3;
    private PlayerHealth playerHealthBar;
    private GameManager gameManager;
    private Animator animator;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        rigidbody = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
        footStept = GetComponent<FootStept>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gunParticle = GameObject.Find("Particle Gun").GetComponent<ParticleSystem>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        playerHealthBar = GameObject.Find("HealthSlider").GetComponent<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        // True if game started
        if (gameManager.GameIsActive)
        {
            // Store the input axes.
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // Move the player around the scene.
            Move();

            // Turn the player to face the mouse cursor.
            Turning();
        }
    }

    private void Update()
    {
        // True if game started
        if (gameManager.GameIsActive)
        {
            // Fire gun left click
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                gunParticle.Play();
                audioSource.PlayOneShot(gunShoot, 0.5f);
                animator.SetBool("isShooting", true);
            }
            if (enemyBite && Time.time >= nextTimeToBite)
            {
                nextTimeToBite = Time.time + 1f / biteRate;
                oneBite = true;
            }
            animator.SetBool("isShooting", false);
        }
    }

    private void Turning()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, camRayLength, floorMask) && !playerStats.isDead())
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
            footStept.IsSprinting();
            movement = movement.normalized * (playerStats.MovementSpeed * playerStats.SprintSpeedModifier) * Time.deltaTime;
            rigidbody.MovePosition(transform.position + movement);
            animator.SetFloat("animSpeed", 2.8f);
        }
        else if(IsWalking())
        {
            // Move the player to it's current position plus the movement.
            footStept.NotSprinting();
            rigidbody.MovePosition(transform.position + movement);
            animator.SetBool("isWalking", true);
            animator.SetFloat("animSpeed", 1f);
        }
        else
        {
            rigidbody.angularVelocity = Vector3.zero;
            animator.SetBool("isWalking", false);
        }
    }

    // Returns a boolean that is true if either of the input axes is non-zero.
    public bool IsWalking()
    {
        return horizontal != 0f || vertical != 0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        // If enemy layer collided with the object, damage the player by the amount
        if(collision.gameObject.layer == 11)
        {
            enemyBite = true;

            // Damage player's HP by damage amount
            if (enemyBite && oneBite)
            {
                EnemyStats enemyStats = collision.gameObject.GetComponent<EnemyStats>();
                playerStats.DamagePlayer(enemyStats.Damage);
                playerHealthBar.UpdateHealth(playerStats.Health);
                soundManager.PlayPlayerTakeDamage();
                enemyStats.HitPlayerAnimation();

                Debug.Log(playerStats.Health);
                oneBite = false;
                enemyBite = false;
            }
        }
    }
    public void PlayPlayerDeathAnimation()
    {
        animator.SetTrigger("isDead");
    }
}
