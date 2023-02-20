using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using UnityEngine.UI;

public class Relais : MonoBehaviour
{
    [SerializeField] private int QuantitéJoueurs;
    [SerializeField] private TMP_InputField codeAccès;
    [SerializeField] private Button boutonConnexionClient;
    [SerializeField] private Button boutonConnexionHote;

    private void Awake()
    {
        boutonConnexionClient.onClick.AddListener(JoinRelay);
        boutonConnexionHote.onClick.AddListener(CreateRelay);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }
        
    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(QuantitéJoueurs - 1);
            
            string joinCode =  await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            Debug.Log("Join Code: " + joinCode);
            
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            NetworkManager.Singleton.StartHost();

            DésactiverInterfacePM();

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
        
    private async void JoinRelay()
    {
        try
        {
            Debug.Log("Connexion au relais avec : " + codeAccès.text );
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(codeAccès.text);
            
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            DésactiverInterfacePM();
        }
        catch (RelayServiceException r)
        {
            Debug.Log(r);
        }
    }

    private void DésactiverInterfacePM()
    {
        boutonConnexionClient.gameObject.SetActive(false);
        codeAccès.gameObject.SetActive(false);
        boutonConnexionHote.gameObject.SetActive(false);
        Cursor.visible = false;
    }
}
