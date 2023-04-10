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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer == layeraccelere)
        {
            isInZone = true;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            gravity *= 10f;
        }
        if (layer == layerinverse)
        {
            isInZone = true;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            gravity *= -6f;
        }
        if (layer == layeraccelereinverse)
        {
            isInZone = true;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            
            gravity *= -12;
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
        gravity = Physics.gravity / 3;
        _rigidbody.useGravity = true;
    }
}
