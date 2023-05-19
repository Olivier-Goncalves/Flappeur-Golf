using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
// Fait par: Olivier Gonçalves
public class GestionCaméraRéseau : NetworkBehaviour
{
    private Camera camera;
    private AudioListener audio;
    private void Awake()
    {
        camera = GetComponent<Camera>();
        audio = GetComponent<AudioListener>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            camera.enabled = false;
            audio.enabled = false;
        }
        else
        {
            camera.enabled = true;
        }
    }
}
