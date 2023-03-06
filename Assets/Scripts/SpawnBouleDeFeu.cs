using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnBouleDeFeu : MonoBehaviour
{
    [SerializeField] private GameObject bouleDeFeu;

    private void Awake()
    {
        GameObject bGameObject = Instantiate(bouleDeFeu, transform.position, quaternion.identity);
        bGameObject.transform.SetParent(transform);
    }
}