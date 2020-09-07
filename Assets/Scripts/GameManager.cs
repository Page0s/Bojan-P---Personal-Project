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
    private float slowdownFactor = 0.05f;
    private int destroyedSpawners;
    private bool spawningEnemysEnded = false;
    private PlayerStats playerStats;
    private Spawner[] spawners;
    private SoundManager soundManager;
    private ParticleGun particleGun;

    private void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        particleGun = GameObject.Find("Particle Gun").GetComponent<ParticleGun>();
        spawners = FindObjectsOfType<Spawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
    }

    private IEnumerator SpawnEnemyWave()
    {
        // Play new Wave sound
        soundManager.PlayNewWaveSound();

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
            soundManager.PlayePlayerDeath();
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
        StartCoroutine(SlowMotionEndGame());
    }

    private IEnumerator SlowMotionEndGame()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSecondsRealtime(4);
        Time.timeScale = 0;
        soundManager.StopAllAudioEffects();
    }

    // Damage enemy's HP by projectile damage amount
    public void DamageEnemy(GameObject other, int damage)
    {
        EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();

        enemyStats.DamageEnemy(damage);
        soundManager.PlayEnemyTakeDamageSound();

        Debug.Log(enemyStats.Health);

        // Enemy dies if HP is 0
        if (enemyStats.isDead())
        {
            // Adds enemy XP to player total XP
            playerStats.AddExperience(enemyStats.ExperienceValue);
            soundManager.PlayEnemyDeathSound();
            Destroy(other.gameObject);

            Debug.Log("Enemy died!");
            Debug.Log($"Player XP: {playerStats.Experience}");
            Debug.Log($"Player Level: {playerStats.PlayerLevel}");
        }
    }

    // Damage spawner's HP by projectile damage amount
    public void DamageSpawner(GameObject other, int damage)
    {
        Spawner spawner = other.gameObject.GetComponent<Spawner>();

        spawner.DamageSpawner(damage);

        Debug.Log($"Spawner HP: {spawner.Health}");

        // Adds spawner XP to player total XP
        if (spawner.isDead())
        {
            Debug.Log("Destroyed Spawners: " + destroyedSpawners);
            ++destroyedSpawners;
            Debug.Log($"Destroyed Spawners: {destroyedSpawners}");

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
                playerStats.MovementSpeed += 1;
                Debug.Log($"Current player movement speed: {playerStats.MovementSpeed}");
                break;
            case 2:
                Debug.Log($"Player spring speed modifier: {playerStats.SprintSpeedModifier}");
                playerStats.SprintSpeedModifier += 0.1f;
                Debug.Log($"Current player sprint movement modifier: {playerStats.SprintSpeedModifier}");
                break;
            case 3:
                Debug.Log($"Player wepon damage: {particleGun.Damage}");
                particleGun.Damage += 10;
                Debug.Log($"Player wepon damage: {particleGun.Damage}");
                break;
            default:
                break;
        }
    }
}
