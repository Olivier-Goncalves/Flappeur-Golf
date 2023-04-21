using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
// Fait par: Guillaume Flamand
public class SpawnBouleDeFeu : MonoBehaviour
{
    [SerializeField] private GameObject bouleDeFeu;
    private void Start()
    {
        Transform[] childs = transform.GetComponentsInChildren<Transform>();
        Transform transformToSpawn = transform;
        foreach (var transform in childs)
        {
            if (transform.gameObject.name == "positionSpawn")
            {
                transformToSpawn = transform;
            }
        }
        GameObject boule = Instantiate(bouleDeFeu, transformToSpawn.position, quaternion.identity);
        
        boule.transform.SetParent(transform);
    }
}