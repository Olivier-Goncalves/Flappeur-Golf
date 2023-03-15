using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine.UI;

// Fait par: Olivier Gonçalves
public class Relay : MonoBehaviour
{
    public static int QuantitéJoueurs;
    
    [SerializeField] private TMP_InputField inputJoinCode;
    [SerializeField] private Button btnCréerRelais;
    [SerializeField] private Button btnJoindreRelais;
    [SerializeField] private TMP_Text textJoinCode;
    [SerializeField ]private Canvas canvasRelayUI;
    [SerializeField] private Canvas canvasJoinCodeUI;
    private void Awake()
    {
        btnCréerRelais.onClick.AddListener(CreateRelay);
        btnJoindreRelais.onClick.AddListener(JoinRelay);
        canvasRelayUI = GetComponentInChildren<Canvas>();
        canvasJoinCodeUI.enabled = false;
        Cursor.visible = true;
        QuantitéJoueurs = 2;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
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
            textJoinCode.text = "Join Code: " + joinCode;
            canvasJoinCodeUI.enabled = true;
            
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            NetworkManager.Singleton.StartHost();
            
            canvasRelayUI.enabled = false;
            Cursor.visible = false;
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
            Debug.Log("Joining Relay with : " + inputJoinCode.text);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(inputJoinCode.text);
            
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            canvasRelayUI.enabled = false;
        }
        catch (RelayServiceException r)
        {
            Debug.Log(r);
        }
    }
    
}
