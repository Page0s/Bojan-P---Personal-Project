using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    private Slider slider;
    private TMP_Text experienceText;
    private float maxExperience = 0;
    private float currentExperience = 0;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        experienceText = GameObject.Find("ExperienceText").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentExperience = maxExperience;
        experienceText.text = currentExperience.ToString();
        slider.value = CalculateExperience();
    }

    private float CalculateExperience()
    {
        return currentExperience;
    }

    public void UpdateExperience(float amount)
    {
        currentExperience = amount;
        experienceText.text = currentExperience.ToString();
        slider.value = CalculateExperience();
    }
}
