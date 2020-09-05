using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health { get => health; }
    public int Experience { get; private set; }
    public int PlayerLevel { get; private set; }
    public float SpeedModifier { get; private set; }
    public float MovementSpeed { get; private set; }

    [SerializeField] private int health = 100;
    [SerializeField] private int stamina = 100;
    [SerializeField] private float speedModifier = 1.5f;
    [SerializeField] private float movementSpeed = 10f;

    private void Start()
    {
        PlayerLevel = 1;
    }

    // Damage player's HP by projectile damage amount
    public void Damage(int damage)
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
        }
    }
}
