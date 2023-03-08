using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
// Fait par: Guillaume Flamand
public class SpawnBouleDeFeu : MonoBehaviour
{
    [SerializeField] private GameObject bouleDeFeu;

    private void Awake()
    {
        GameObject boule = Instantiate(bouleDeFeu, transform.position, quaternion.identity);
        boule.transform.SetParent(transform);
    }
}