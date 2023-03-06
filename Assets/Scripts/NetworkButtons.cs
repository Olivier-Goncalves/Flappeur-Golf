using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkButtons : MonoBehaviour {
    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            if (GUILayout.Button("Host"))
            {
                NetworkManager.Singleton.StartHost();
                // Relay.relais.CreateRelay();
            }
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        GUILayout.EndArea();
    }

    // private void Awake() {
    //     GetComponent<UnityTransport>().SetDebugSimulatorParameters(
    //         packetDelay: 120,
    //         packetJitter: 5,
    //         dropRate: 3);
    // }
}