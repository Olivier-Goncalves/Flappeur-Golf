using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Collections;
using Unity.Services.Authentication;

// Fait par: Louis-Félix Clément
public class GestionMenuMultijoueur : NetworkBehaviour
{
    [SerializeField] private Button boutonRetour;
    [SerializeField] private Button boutonCommencer;
    [SerializeField] private Transform[] PointsDépart;
    public int index { get; set; }
    private GameObject[] allPlayers;
    private GameObject playerServer;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        boutonRetour.onClick.AddListener(GénérerScèneRetour);   
        boutonCommencer.onClick.AddListener(DémarrerPartieClientRpc);
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerServer = allPlayers[0];
    }
    [ClientRpc]
    public void DémarrerPartieClientRpc()
    {
        
        // = PointsDépart[0].position;
        // index++;
        Debug.Log("Client RPC Appelé");
        gameObject.transform.position = PointsDépart[0].position;
    }

    private void GénérerScèneRetour()
    {
        SceneManager.LoadScene("MenuAccueil");
        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Cursor.visible = true;
    }

    
}
