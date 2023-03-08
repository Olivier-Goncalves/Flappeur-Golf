using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] private Vector3 respawn;
    private const int layerBouleDeFeu = 9;
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.gameObject.layer == layerBouleDeFeu)
        {
            Ressusciter();
        }
    }

    private void Ressusciter()
    {
        gameObject.transform.position = respawn;
    }
}
