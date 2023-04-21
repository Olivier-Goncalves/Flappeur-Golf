using System;
using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Complex;
using Shapes2D;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class TeleporterJeu : NetworkBehaviour
{
    private List<Transform> spawns;

    private void Awake()
    {
        spawns = GameObject.Find("GestionnaireJeu").GetComponent<GestionJeuMultijoueur>().spawns;
    }

    public void Teleporter(int index)
    {
        if (IsOwner)
        {
            transform.position = spawns[index].position;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            // Debug.Log("Téléporter est appellé avec l'index: " + index);
        }
    }
}
