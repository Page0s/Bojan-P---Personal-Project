using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private float timeBetweenEnemySpawn = 2f;

    private float enemyAmount = 5;
    private bool spawningEnemysEnded = false;
    private bool gameEnded = false;
    private PlayerStats playerStats;
    private Spawner[] spawners;
    private int destroyedSpawners;

    private void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        spawners = FindObjectsOfType<Spawner>();
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
            yield return new WaitForSeconds(timeBetweenEnemySpawn);
        }
        spawningEnemysEnded = true;
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
        if (spawningEnemysEnded)
        {
            Debug.Log("Stop spawning enemys!");
            StopCoroutine(SpawnEnemyWave());
        }

        if (playerStats.isDead())
        {
            Debug.Log("You Lose! Player died!");
            spawningEnemysEnded = true;
            EndGame();
        }
        else if(destroyedSpawners == spawners.Length)
        {
            Debug.Log("You Win! All spawners destroyed!");
            spawningEnemysEnded = true;
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;
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

        playerStats.DamagePlayer(enemyStats.Damage);

        Debug.Log(playerStats.Health);
    }

    // Damage spawner's HP by projectile damage amount
    public void DamageSpawner(GameObject other, int damage)
    {
        ++destroyedSpawners;

        Spawner spawner = other.gameObject.GetComponent<Spawner>();

        spawner.DamageSpawner(damage);

        Debug.Log($"Spawner HP: {spawner.Health}");

        // Adds spawner XP to player total XP
        if (spawner.isDead())
        {
            playerStats.AddExperience(spawner.ExperienceValue);
            other.SetActive(false);

            Debug.Log("Spawned destroyed!");
            Debug.Log($"Player XP: {playerStats.Experience}");
            Debug.Log($"Player Level: {playerStats.PlayerLevel}");
        }
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
