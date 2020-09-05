using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnLocations;

    private float enemyAmount = 5;
    private bool endGame = false;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
    }

    private IEnumerator SpawnEnemyWave()
    {
        for(int i = 0; i < enemyAmount; ++i)
        {
            spawnEnemy();
            yield return new WaitForSeconds(2);
        }
        endGame = true;
    }

    private void spawnEnemy()
    {
        Transform spawnLocation = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Length)];
        Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Stop the game when endGame == true
        if (endGame)
        {
            Debug.Log("Stop spawning enemys!");
            StopCoroutine(SpawnEnemyWave());
        }

        if (playerStats.isDead())
        {
            endGame = true;
            Debug.Log("Game Over!");
        }
    }

    // Damage enemy's HP by projectile damage amount
    public void DamageEnemy(GameObject other, int damage)
    {
        EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();

        enemyStats.DamageEnemy(damage);

        Debug.Log(enemyStats.Health);

        // Enemy dies if HP is 0
        if (enemyStats.isDead())
        {
            // Adds enemy XP to player total XP
            playerStats.AddExperience(enemyStats.ExperienceValue);
            Destroy(other.gameObject);

            Debug.Log("Enemy died!");
            Debug.Log($"Player XP: {playerStats.Experience}");
            Debug.Log($"Player Level: {playerStats.PlayerLevel}");
        }
    }

    // Damage player's HP by projectile damage amount
    public void DamagePlayer(Collision collision, int health)
    {
        EnemyStats enemyStats = collision.gameObject.GetComponent<EnemyStats>();

        playerStats.Damage(enemyStats.Damage);

        Debug.Log(playerStats.Health);
    }

    public void ModifyAbility(int abilityIndex)
    {
        switch (abilityIndex)
        {
            case 1:
                Debug.Log($"Player was moving with speed: {playerStats.MovementSpeed}");
                playerStats.MovementSpeed = 20;
                Debug.Log($"Current player movement speed: {playerStats.MovementSpeed}");
                break;
            case 2:
                Debug.Log($"Player spring speed modifier: {playerStats.SprintSpeedModifier}");
                playerStats.SprintSpeedModifier = 2f;
                Debug.Log($"Current player sprint movement modifier: {playerStats.SprintSpeedModifier}");
                break;
            case 3:
                Debug.Log($"Player wepon damage: ");
                break;
            default:
                break;
        }
    }
}
