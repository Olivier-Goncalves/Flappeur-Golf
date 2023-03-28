using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Collections;
using Unity.Services.Authentication;

// Fait par: Louis-F�lix Cl�ment
public class GestionMenuMultijoueur : NetworkBehaviour
{
    [SerializeField] private Button boutonRetour;
    [SerializeField] private Button boutonCommencer;
    [SerializeField] private Transform[] PointsD�part;
    public int index { get; set; }
    private GameObject[] allPlayers;
    private GameObject playerServer;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        boutonRetour.onClick.AddListener(G�n�rerSc�neRetour);   
        boutonCommencer.onClick.AddListener(D�marrerPartieClientRpc);
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerServer = allPlayers[0];
    }
    [ClientRpc]
    public void D�marrerPartieClientRpc()
    {
        
        // = PointsD�part[0].position;
        // index++;
        Debug.Log("Client RPC Appel�");
        gameObject.transform.position = PointsD�part[0].position;
    }

    private void G�n�rerSc�neRetour()
    {
        SceneManager.LoadScene("MenuAccueil");
        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Cursor.visible = true;
    }

    
}
