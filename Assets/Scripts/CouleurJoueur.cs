using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CouleurJoueur : NetworkBehaviour
{
    private Material material;
    private Color[] colors = new[]
    {
        Color.blue, Color.black, Color.red, Color.green, Color.cyan, Color.magenta, Color.yellow, Color.gray,
        Color.white
    };
    
    public override void OnNetworkSpawn()
    {
        int idJoueur = (int)OwnerClientId;
        material.SetColor("_Color", colors[idJoueur]);
        GetComponentInChildren<TrailRenderer>().startColor = colors[idJoueur];
    }

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
}
