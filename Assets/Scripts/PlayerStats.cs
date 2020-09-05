using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health { get => health; }

    [SerializeField] private int health = 100;
    [SerializeField] private int stamina = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
