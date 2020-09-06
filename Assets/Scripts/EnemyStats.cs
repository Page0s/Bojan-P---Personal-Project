using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int Health { get => health; }
    public int Damage { get => damage; }

    public int ExperienceValue { get => experienceValue; }

    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 20;
    [SerializeField] private int experienceValue = 10;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Damage enemy's HP by projectile damage amount
    public void DamageEnemy(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
    }

    public bool isDead() => health <= 0;
}
