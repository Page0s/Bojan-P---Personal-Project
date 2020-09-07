using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private Slider slider;
    private TMP_Text healthText;
    private float maxHealth = 100;
    private float currentHealth;
    private string originText = "HP :";

    private void Awake()
    {
        slider = GetComponent<Slider>();
        healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = originText + currentHealth.ToString();
        slider.value = CalculateHealth();
    }

    private float CalculateHealth()
    {
        return currentHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth = amount;
        healthText.text = originText + currentHealth.ToString();
        slider.value = CalculateHealth();
    }
}
