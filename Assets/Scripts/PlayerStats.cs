using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int Health { get => health; }
    public int Experience { get; private set; }
    public int PlayerLevel { get; private set; }
    public float SprintSpeedModifier { get; set; }
    public float MovementSpeed { get; set; }
    public bool AbilityButtonVissible { get; set; }

    [SerializeField] private int health = 100;
    [SerializeField] private int stamina = 100;
    [SerializeField] private AudioClip levelUpSound;
    [SerializeField] private ParticleSystem LevelUpParticles;

    private AudioSource audioSource;
    private TMP_Text levelText;
    private ExperienceBar experienceBar;
    private Coroutine abilityButtons;

    private void Awake()
    {
        PlayerLevel = 1;
        MovementSpeed = 5f;
        SprintSpeedModifier = 1.5f;
        audioSource = GetComponent<AudioSource>();
        levelText = GameObject.Find("LevelText").GetComponent<TMP_Text>();
        experienceBar = GameObject.Find("ExperienceSlider").GetComponent<ExperienceBar>();
    }

    // Damage player's HP by projectile damage amount
    public void DamagePlayer(int damage)
    {
        if(health > 0)
        {
            health -= damage;
        }
    }

    public bool isDead() => health <= 0;

    public void AddExperience(int amount)
    {
        Experience += amount;
        experienceBar.UpdateExperience(Experience);

        // DING!!!
        if (Experience >= 100)
        {
            Experience -= 100;
            PlayerLevel += 1;
            audioSource.PlayOneShot(levelUpSound, 1.5f);
            LevelUpParticles.Play();
            levelText.text = PlayerLevel.ToString();
            experienceBar.UpdateExperience(Experience);
            // Ability Selection
            StopTimeAbilitySelection();
        }
    }

    private void StopTimeAbilitySelection()
    {
        // First show abulity buttons
        AbilityButtonVissible = true;

        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button button in buttons)
        {
            Debug.Log($"Button name: {button.name}");
            if (button.gameObject.CompareTag("AbilityModiffier"))
                button.gameObject.SetActive(true);
        }
        // Second Stop time
        Time.timeScale = 0f;
        // Wait button selection
        abilityButtons = StartCoroutine(AbilitySelection());
        StopCoroutine(abilityButtons);
    }

    private IEnumerator AbilitySelection()
    {
        while (true)
        {
            // wait untill player press the ability button
            if (!AbilityButtonVissible) break;
            yield return null;
        }
    }
}
