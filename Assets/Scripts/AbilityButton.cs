using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private int abilityIndex = 1;
    private GameManager gameManager;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //button = GetComponent<Button>();
        //button.onClick.AddListener(AbilityModifier);
    }

    private void AbilityModifier()
    {
        //Debug.Log($"{button.gameObject.name} was clicked!");
        gameManager.ModifyAbility(abilityIndex);
    }
}
