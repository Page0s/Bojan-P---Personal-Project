using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGun : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collided!");
    }
}
