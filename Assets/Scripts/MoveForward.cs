using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveForward : MonoBehaviour
{
    [SerializeField] private float forceAmount = 5f;

    private Transform playerTransform;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move enemy to player position
        rigidbody.MovePosition(transform.position + GetDirection().normalized * forceAmount * Time.deltaTime);

        // Rotate to player position
        rigidbody.MoveRotation(GetDeltaRotation());
    }

    private Quaternion GetDeltaRotation()
    {
        return Quaternion.LookRotation(GetDirection());
    }

    private Vector3 GetDirection()
    {
        return playerTransform.position - transform.position;
    }
}
