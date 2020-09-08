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

    private SoundManager soundManager;
    private float counter;
    private Animator animator;

    private void Awake()
    {
        counter = 4;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        counter -= Time.deltaTime;

        if(counter <= 0)
        {
            soundManager.PlayEnemyIdleSound();
            counter = Random.Range(4, 12);
        }
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

    public void HitPlayerAnimation()
    {
        animator.SetTrigger("hitPlayer");
    }
}
