using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fusil: NetworkBehaviour
{
    [SerializeField] private KeyCode toucheTirer;
    [SerializeField] private GameObject balle;
    [SerializeField] private Transform positionSortie;
    private int compteur;
    private int nombreMaxMunition = 3;
    private GameObject balleInstantie;
    private NetworkObject balleReseau;

    private void Awake()
    {
        balleInstantie.GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if(compteur < nombreMaxMunition)
        {
            if (Input.GetKeyDown(toucheTirer))
            {
                ++compteur;
                balleInstantie= Instantiate(balle, positionSortie.position, positionSortie.rotation);
                SpawnDespawnBalleServerRpc(true);
            }   
        }
        else
        {
            SpawnDespawnBalleServerRpc(false);
        }
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnDespawnBalleServerRpc(bool spawn)
    {
        if(spawn)
            balleReseau.Spawn();
        else
        {
            balleReseau.Despawn();
        }
    }
}
