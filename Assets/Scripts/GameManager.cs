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
        if (endGame)
        {
            StopCoroutine(SpawnEnemyWave());
        }
    }
}
