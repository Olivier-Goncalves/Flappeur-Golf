using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Balle : MonoBehaviour
{
    private float time = 0;
    private NetworkObject balle;
    private void Awake()
    {
        balle = GetComponent<NetworkObject>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 5)
        {
            despawnBulletServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void despawnBulletServerRpc()
    {
        balle.Despawn();
    }
}
