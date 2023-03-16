using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravité : MonoBehaviour
{
    private Vector3 gravity;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        gravity = Physics.gravity / 2;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
    
    void ReverseGravity() => gravity *= -6f;

    private void OnTriggerEnter(Collider other)
    {
        ReverseGravity();
    }

    private void OnTriggerExit(Collider other)
    {
        gravity = Physics.gravity / 2 ;
    }
}
