using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGun : MonoBehaviour
{
    public int Damage { get => damage; }

    [SerializeField] private int damage = 50;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        // If enemy layer collided with the object, damage the enemy by amount
        if (other.gameObject.layer == 11)
        {
            gameManager.DamageEnemy(other, damage);
        }
        else if (other.gameObject.layer == 12)
        {
            gameManager.DamageSpawner(other, damage);
        }
    }
}
