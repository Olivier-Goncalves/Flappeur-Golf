using System;
using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using Unity.Netcode;
using UnityEngine;

public class Pouvoir : NetworkBehaviour
{
    private int pouvoirLayer = 15;
    private float elapsedTime;
    [SerializeField] private GameObject fusil;
    private GameObject nouveauFusil;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == pouvoirLayer)
        {
            GetComponent<Saut>().jumpStrength = UnityEngine.Random.Range(0, 2) == 0 ? 25 : 150;
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 16)
        {
            if (IsOwner)
            {
                Transform[] position = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < position.Length; ++i)
                {
                    if (position[i].gameObject.name == "EmplacementGun")
                    {
                        nouveauFusil = Instantiate(fusil, position[i].position, transform.rotation);
                        SpawnCanonServerRpc();

                        // nouveauFusil.GetComponent<NetworkObject>().Spawn();
                    }
                }

                // despawnCanonServerRpc(other.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SpawnCanonServerRpc()
    {
        nouveauFusil.GetComponent<NetworkObject>().Spawn();
        nouveauFusil.transform.SetParent(gameObject.transform);
    }
    // [ServerRpc(RequireOwnership = false)]
    // private void despawnCanonServerRpc(GameObject canon)
    // {
        // canon.GetComponent<NetworkObject>().Despawn();
    // }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 7.5f)
        {
            elapsedTime = 0;
            GetComponent<Saut>().jumpStrength = 100;
        }
    }
}
