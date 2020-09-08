using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool GameIsActive { get => gameIsActive; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private float timeBetweenEnemySpawn = 2f;
    [SerializeField] Button restartButton;

    private float slowdownFactor = 0.05f;
    private float spawnAmount = 10f;
    private int destroyedSpawners;
    private int spawnersLeft = 4;
    private int waveCounter;
    private bool spawningEnemysEnded = false;
    private bool gameIsActive;
    private PlayerStats playerStats;
    private SoundManager soundManager;
    private ParticleGun particleGun;
    private TMP_Text spawnerText;
    private TMP_Text gameOverText;
    private TMP_Text enemyCounterText;
    private TMP_Text waveText;
    private string originSpawnerText = "Spawners Left: ";
    private AudioSource playerAudioSource;
    private int enemyCounter;
    private Coroutine spawnEnemysCoroutine;
    private Spawner[] spawners;
    private List<Transform> spawnerNames;

    private void Awake()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerAudioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        particleGun = GameObject.Find("Particle Gun").GetComponent<ParticleGun>();
        spawnerText = GameObject.Find("SpawnerText").GetComponent<TMP_Text>();
        gameOverText = GameObject.Find("Game Over").GetComponent<TMP_Text>();
        enemyCounterText = GameObject.Find("EnemyText").GetComponent<TMP_Text>();
        waveText = GameObject.Find("WaveText").GetComponent<TMP_Text>();
        spawners = FindObjectsOfType<Spawner>();
        spawnerNames = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnLocations.Length; ++i)
            spawnerNames.Add(spawnLocations[i]);
    }

    public void StartGame(int difficultyAmount, int gunDamage)
    {
        gameIsActive = true;
        spawnAmount = difficultyAmount;
        particleGun.Damage = gunDamage;
        playerAudioSource.Play();
        spawnerText.text = originSpawnerText + spawnersLeft.ToString();
        spawnEnemysCoroutine = StartCoroutine(SpawnEnemyWave());
    }

    private IEnumerator SpawnEnemyWave()
    {
        // Play new Wave sound
        soundManager.PlayNewWaveSound();
        ++waveCounter;
        waveText.text = waveCounter.ToString();

        for(int i = 0; i < spawnAmount; ++i)
        {
            spawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemySpawn);
        }
        spawningEnemysEnded = true;
        Invoke("RestartWave", 10f);
    }

    private void RestartWave()
    {
        spawnAmount *= 1.5f;
        spawnEnemysCoroutine = StartCoroutine(SpawnEnemyWave());
        spawningEnemysEnded = false;
        Debug.Log($"Starting {waveCounter} Wave!");
    }

    // Finds next available spawn location for enemys
    private void spawnEnemy()
    {
        Transform spawnLocation = spawnerNames.ElementAt(UnityEngine.Random.Range(0, spawnerNames.Count));

        Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Stop the game when endGame == true
        if (spawningEnemysEnded)
        {
            Debug.Log("Stop spawning enemys!");
            StopCoroutine(spawnEnemysCoroutine);
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
        gameOverText.enabled = true;
        restartButton.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            ++enemyCounter;
            // Adds enemy XP to player total XP
            playerStats.AddExperience(enemyStats.ExperienceValue);
            soundManager.PlayEnemyDeathSound();
            enemyCounterText.text = enemyCounter.ToString();
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
            spawnerText.text = originSpawnerText + (spawnersLeft - destroyedSpawners).ToString();

            Debug.Log($"Destroyed {spawner.gameObject.name} Spawner!");
            Debug.Log($"Destroyed Spawners: {destroyedSpawners}");
            // remove spawner from spawner locations
            spawnerNames.RemoveAt(FindIndex(spawner.gameObject.name));
           
            // add XP to playr from spawner
            playerStats.AddExperience(spawner.ExperienceValue);
            other.SetActive(false);

            Debug.Log("Spawned destroyed!");
            Debug.Log($"Player XP: {playerStats.Experience}");
            Debug.Log($"Player Level: {playerStats.PlayerLevel}");
        }
    }

    // finds the destroyed spawner and remove it from the spawner location list
    private int FindIndex(string spawnerName)
    {
        foreach(Transform transformSpawner in spawnerNames)
        {
            if (transformSpawner.gameObject.name == spawnerName)
                return spawnerNames.IndexOf(transformSpawner);
        }
        return 0;
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
