using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravité : MonoBehaviour
{
    private int layeraccelere = 11;
    private int layerinverse = 12;
    private int layeraccelereinverse = 13;
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
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == layeraccelere)
        {
            gravity *= 10f;
        }
        if (other.gameObject.layer == layerinverse)
        {
            gravity *= -6f;
        }
        if (other.gameObject.layer == layeraccelereinverse)
        {
            gravity *= -12;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        gravity = Physics.gravity / 2 ;
    }
}
