﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private int SpawnInFirstWave;
    [SerializeField] private int gunDamage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rateOfFire;

    private Button button;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    private void SetDifficulty()
    {
        Debug.Log($"{button.gameObject.name} button was clicked");
        gameManager.StartGame(SpawnInFirstWave, gunDamage, movementSpeed, rateOfFire);

        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
            button.gameObject.SetActive(false);
    }
}
