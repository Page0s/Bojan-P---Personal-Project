using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider slider;
    private float maxHealth = 100;
    private float currentHealth;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        slider.value = CalculateHealth();
    }

    private float CalculateHealth()
    {
        //return currentHealth / maxHealth;
        return currentHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth = amount;
        slider.value = CalculateHealth();
    }
}
