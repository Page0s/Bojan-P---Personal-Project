using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStept : MonoBehaviour
{
    public float WalkingRate { get; set; }

    [SerializeField] AudioClip[] walkSteps;
    [SerializeField] AudioSource audioSource;

    PlayerController playerController;
    float nextTimeToStep;
    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        WalkingRate = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsWalking() && Time.time >= nextTimeToStep)
        {
            nextTimeToStep = Time.time + 1f / WalkingRate;

            audioSource.volume = Random.Range(0.8f, 1f);
            audioSource.pitch = Random.Range(0.7f, 1.1f);
            audioSource.PlayOneShot(walkSteps[Random.Range(0, walkSteps.Length)], 0.6f);
        }
    }

    internal void IsSprinting()
    {
        WalkingRate = 5;
    }

    internal void NotSprinting()
    {
        WalkingRate = 3;
    }
}
