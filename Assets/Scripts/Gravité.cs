using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fait pas Guillaume Flamand
public class Gravité : MonoBehaviour
{
    private int layeraccelere = 11;
    private int layerinverse = 12;
    private int layeraccelereinverse = 13;
    private Vector3 gravity;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        gravity = Physics.gravity / 4;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer == layeraccelere)
        {
            gravity *= 10f;
        }
        if (layer == layerinverse)
        {
            gravity *= -6f;
        }
        if (layer == layeraccelereinverse)
        {
            gravity *= -12;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        gravity = Physics.gravity / 4 ;
    }
}
