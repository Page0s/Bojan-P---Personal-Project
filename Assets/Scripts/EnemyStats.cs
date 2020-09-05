using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int Health { get => health; }
    public int Damage { get => damage; }

    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 20;

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
