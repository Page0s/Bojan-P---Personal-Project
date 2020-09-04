using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveForward : MonoBehaviour
{
    [SerializeField] private float forceAmount = 5f;

    private Rigidbody rigidbody;
    private Vector3 newMovement;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position + (transform.forward * Time.deltaTime * forceAmount));
    }
}
