using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApplyForceOverTime : MonoBehaviour
{
    [SerializeField] private Vector3 forceToApply;
    private Rigidbody rb;
    private Vector3 forceToApplyInWorldSpace;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddRelativeForce(forceToApply * Time.deltaTime);
    }
}