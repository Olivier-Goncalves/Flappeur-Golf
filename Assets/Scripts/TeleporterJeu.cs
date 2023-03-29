using System;
using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class TeleporterJeu : NetworkBehaviour
{
    private Transform[] spawns;

    private void Awake()
    {
        spawns = GameObject.Find("GestionnaireJeu").GetComponent<GestionJeuMultijoueur>().spawns;
    }

    public void Teleporter(int index)
    {
        GameObject[] joueurs = GameObject.FindGameObjectsWithTag("Player");
        foreach (var joueur in joueurs)
        {
            if (IsOwner)
            {
                joueur.transform.position = spawns[index].position;
            }
        }

    }
}
