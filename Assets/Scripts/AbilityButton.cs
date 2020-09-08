using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private int modifficationSelection;

    private GameManager gameManager;
    private Button button;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        button = GetComponent<Button>();
        button.onClick.AddListener(AbilityModifier);
    }

    private void AbilityModifier()
    {
        Debug.Log($"{button.gameObject.name} was clicked!");
        gameManager.ModifyAbility(modifficationSelection);

        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            if (button.gameObject.CompareTag("AbilityModiffier"))
                button.gameObject.SetActive(false);
        }
        // Return tome to normal
        Time.timeScale = 1f;
        playerStats.AbilityButtonVissible = false;
    }
}
