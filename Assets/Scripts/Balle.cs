using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Balle : MonoBehaviour
{
    private float time = 0;
    // Update is called once per frame
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
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
