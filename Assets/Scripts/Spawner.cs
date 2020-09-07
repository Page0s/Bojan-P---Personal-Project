using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int Health { get => health; }
    public int ExperienceValue { get => experienceValue; }

    [SerializeField] private int health = 500;
    [SerializeField] private int experienceValue = 110;

    public void DamageSpawner(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
    }

    public bool isDead() => health <= 0;
}
