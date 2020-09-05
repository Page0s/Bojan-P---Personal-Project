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

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
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
        other.gameObject.GetComponent<EnemyStats>().DamageEnemy(damage);

        Debug.Log(other.gameObject.GetComponent<EnemyStats>().Health);

        // Enemy dies if HP is 0
        if (other.gameObject.GetComponent<EnemyStats>().isDead())
        {
            Destroy(other.gameObject);

            Debug.Log("Enemy died!");
        }
    }

    // Damage player's HP by projectile damage amount
    internal void DamagePlayer(Collision collision, int health)
    {
        playerStats.Damage(collision.gameObject.GetComponent<EnemyStats>().Damage);

        Debug.Log(playerStats.Health);
    }
}
