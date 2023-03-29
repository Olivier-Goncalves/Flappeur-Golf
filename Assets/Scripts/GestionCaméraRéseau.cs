using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
// Fait par: Olivier Gonçalves
public class GestionCaméraRéseau : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            GetComponent<Camera>().enabled = true;
        }
    }
}
