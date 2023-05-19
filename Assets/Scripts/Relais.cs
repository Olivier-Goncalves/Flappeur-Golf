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
// https://www.youtube.com/watch?v=msPNJ2cxWfw&t=485s
public class Relais : MonoBehaviour
{
    public static int QuantitéJoueursMax;
    
    [SerializeField] private TMP_InputField inputJoinCode;
    [SerializeField] private Button btnCréerRelais;
    [SerializeField] private Button btnJoindreRelais;
    [SerializeField] private TMP_Text texteCode;
    [SerializeField ]private Canvas canvasRelais;
    [SerializeField] private Canvas canvasCodeUI;
    private void Awake()
    {
        btnCréerRelais.onClick.AddListener(CreerRelais);
        btnJoindreRelais.onClick.AddListener(JoindreRelais);
        canvasRelais = GetComponentInChildren<Canvas>();
        canvasCodeUI.enabled = false;
        Cursor.visible = true;
        QuantitéJoueursMax = 8;
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
            Debug.Log("Connecté avec " + AuthenticationService.Instance.PlayerId);
        };
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    
    private async void CreerRelais()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(QuantitéJoueursMax - 1);
            
            string joinCode =  await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            texteCode.text = "Code: " + joinCode;
            canvasCodeUI.enabled = true;
            
            RelayServerData donneesRelaisServeur = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(donneesRelaisServeur);
            
            NetworkManager.Singleton.StartHost();
            
            canvasRelais.enabled = false;
            Cursor.visible = false;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    private async void JoindreRelais()
    {
        try
        {
            Debug.Log("En train de joindre avec : " + inputJoinCode.text);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(inputJoinCode.text);
            
            RelayServerData donneesRelaisServeur = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(donneesRelaisServeur);

            NetworkManager.Singleton.StartClient();

            canvasRelais.enabled = false;
        }
        catch (RelayServiceException r)
        {
            Debug.Log(r);
        }
    }
    
}
