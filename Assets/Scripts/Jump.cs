using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpStrength = 100;
    private Rigidbody _rigidbody;

    private static int greenLayer = 10;
    private bool isOnGreen = false;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 velocity = _rigidbody.velocity;
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space)) && isOnGreen)
        {
            _rigidbody.velocity = velocity / 1.5f - new Vector3(0, 0, velocity.z / 2);
            _rigidbody.AddRelativeForce(new Vector3(0, 0, jumpStrength * 12));
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            _rigidbody.velocity = velocity / 1.5f - new Vector3(0, velocity.y / 2, 0);
            _rigidbody.AddRelativeForce(new Vector3(0, jumpStrength * 10, jumpStrength * 12));
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.GetComponent<Renderer>().material.name == "Green")
        {
            isOnGreen = true;
        }
    }

    private void OnCollisionExit(UnityEngine.Collision other)
    {
        isOnGreen = false;
    }
}