using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fusil: NetworkBehaviour
{
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private GameObject balle;
    [SerializeField] private Transform exitTransform;
    private int compteur = 0;
    private int nombreMaxMunition = 3;
    private float time = 0;
    private GameObject balleInstantie;
    private void Update()
    {
        if(compteur < nombreMaxMunition)
        {
            if (Input.GetKeyDown(shootKey))
            {
                ++compteur;
                balleInstantie= Instantiate(balle, exitTransform.position, exitTransform.rotation);
                SpawnBalleServerRpc();
                // balleInstantie.GetComponent<NetworkObject>().Spawn();
            }   
        }
        else
        {
            DespawnBalleServerRpc();
            // gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnBalleServerRpc()
    {
        balleInstantie.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc(RequireOwnership = false)]
    private void DespawnBalleServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
