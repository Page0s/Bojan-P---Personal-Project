using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] private AudioClip rainClip;
    [SerializeField] private AudioClip[] thuderClips;

    private ParticleSystem rainParticle;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rainParticle = GetComponent<ParticleSystem>();
        Invoke("StartRain", 10f);       
    }

    private void StartRain()
    {
        audioSource.PlayOneShot(rainClip, 0.4f);
        rainParticle.Play();

        Invoke("PlayThunderSound", 3f);
        Invoke("StopRain", 25f);
    }

    private void StopRain()
    {
        rainParticle.Stop();
        audioSource.Stop();

        Invoke("StartRain", Random.Range(30, 120));
    }

    private void PlayThunderSound()
    {
        audioSource.PlayOneShot(thuderClips[Random.Range(0, thuderClips.Length)], 0.4f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
