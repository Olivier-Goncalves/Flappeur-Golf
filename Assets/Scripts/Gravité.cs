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
    private bool isInZone = false;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        gravity = Physics.gravity;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        isInZone = true;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        
        if (layer == layeraccelere)
        {
            gravity *= 2f;
        }
        if (layer == layerinverse)
        {
            gravity *= -2f;
        }
        if (layer == layeraccelereinverse)
        {
            
            gravity *= -3f;
        }
    }

    private void FixedUpdate()
    {
        if (isInZone)
        {
            _rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gravity = Physics.gravity;
        _rigidbody.useGravity = true;
    }
}
