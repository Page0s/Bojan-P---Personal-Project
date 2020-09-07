using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health { get => health; }
    public int Experience { get; private set; }
    public int PlayerLevel { get; private set; }
    public float SprintSpeedModifier { get; set; }
    public float MovementSpeed { get; set; }

    [SerializeField] private int health = 100;
    [SerializeField] private int stamina = 100;
    [SerializeField] private AudioClip levelUpSound;
    [SerializeField] private ParticleSystem LevelUpParticles;

    private AudioSource audioSource;

    private void Awake()
    {
        PlayerLevel = 1;
        MovementSpeed = 10f;
        SprintSpeedModifier = 1.5f;
        audioSource = GetComponent<AudioSource>();
    }

    // Damage player's HP by projectile damage amount
    public void DamagePlayer(int damage)
    {
        if(health > 0)
        {
            health -= damage;
        }
    }

    public bool isDead() => health <= 0;

    public void AddExperience(int amount)
    {
        Experience += amount;

        // DING!!!
        if(Experience >= 100)
        {
            Experience -= 100;
            PlayerLevel += 1;
            audioSource.PlayOneShot(levelUpSound, 1.5f);
            LevelUpParticles.Play();
        }
    }
}
