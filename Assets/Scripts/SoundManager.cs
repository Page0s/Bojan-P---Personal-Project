using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] enemyTakeDamageClips;
    [SerializeField] private AudioClip[] playerTakeDamageClips;
    [SerializeField] private AudioClip[] playerDeathClips;
    [SerializeField] private AudioClip[] enemyDeathClips;
    [SerializeField] private AudioClip[] enemyIdleClips;
    [SerializeField] private AudioClip[] newWaveSounds;
    [SerializeField] private AudioClip spawnerHitSound;

    [SerializeField] private AudioSource newWaveAudioSource;
    [SerializeField] private AudioSource playerAudioSource;

    private AudioSource audioSource;
    private float soundRepeatRate = 0.8f;
    private float nextTimeToPlaySound = 0f;
    private bool playEnemyTakeDamgeSound = false;
    private bool playEnemyDeathSound;
    private bool playPlayerTakeDamgeSound = false;
    private bool playPlayerDeathSound;
    private bool spawnerTakeDanage;

    internal void PlayEnemyIdleSound()
    {
        audioSource.PlayOneShot(enemyIdleClips[Random.Range(0, enemyIdleClips.Length)], 0.2f);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playEnemyTakeDamgeSound)
        {
            // Play Take damage sounds
            if (playEnemyTakeDamgeSound && Time.time >= nextTimeToPlaySound)
            {
                nextTimeToPlaySound = Time.time + 1f / soundRepeatRate;
                audioSource.PlayOneShot(RandomClip(enemyTakeDamageClips), 0.5f);
                playEnemyTakeDamgeSound = false;
            }
        }

        if (playEnemyDeathSound)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(RandomClip(enemyDeathClips), 0.6f);
            playEnemyDeathSound = false;
            playEnemyTakeDamgeSound = false;
        }

        if (playPlayerTakeDamgeSound)
        {
            playerAudioSource.PlayOneShot(RandomClip(playerTakeDamageClips), 1.3f);
            playPlayerTakeDamgeSound = false;
        }

        if (playPlayerDeathSound)
        {
            playerAudioSource.PlayOneShot(RandomClip(playerDeathClips), 0.2f);
            playPlayerTakeDamgeSound = false;
            playPlayerDeathSound = false;
        }
        if (spawnerTakeDanage)
        {
            audioSource.PlayOneShot(spawnerHitSound, 0.2f);
            spawnerTakeDanage = false;
        }
    }

    // Return random damage sound
    private AudioClip RandomClip(AudioClip[] randomAudioClips)
    {
        return randomAudioClips[Random.Range(0, randomAudioClips.Length)];
    }

    internal void PlayEnemyTakeDamageSound()
    {
        playEnemyTakeDamgeSound = true;
    }

    internal void PlayEnemyDeathSound()
    {
        playEnemyDeathSound = true;
    }

    public void PlayNewWaveSound()
    {
        newWaveAudioSource.PlayOneShot(newWaveSounds[Random.Range(0, newWaveSounds.Length)], 0.2f);
    }

    public void PlayPlayerTakeDamage()
    {
        playPlayerTakeDamgeSound = true;
    }

    public void PlayePlayerDeath()
    {
        playPlayerDeathSound = true;
    }

    public void StopAllAudioEffects()
    {
        playerAudioSource.Stop();
        audioSource.Stop();
    }

    public void PlaySpawnerTakeDamage()
    {
        spawnerTakeDanage = true;
    }
}
