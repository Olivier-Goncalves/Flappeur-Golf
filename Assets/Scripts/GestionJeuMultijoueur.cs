using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GestionJeuMultijoueur : NetworkBehaviour
{
    public Transform[] spawns;
    private int positionArrivé = 0;
    private int indexNiveau = 0;
    [SerializeField] GestionMenuMultijoueur gestionnaireMenus;

    public void CommencerNiveau()
    {
        PlayTimer();
        TeleporterClientRpc();
        // Jouer Décompte
        gestionnaireMenus.JouerDécompte();
        ActiverJoueursClientRpc();
    }
    
    private void PlayTimer()
    {
        Debug.Log("Pars le Timer de 5 secondes");
    }

    public void ArriverTrou()
    {
        gestionnaireMenus.AfficherPositionArrivée(positionArrivé);
        if(positionArrivé + 1 == NetworkManager.ConnectedClientsList.Count)
        {
            AfficherClassementClientRpc();
            CommencerNiveau();
        }
    }
    [ClientRpc]
    private void AfficherClassementClientRpc()
    {
        Debug.Log("Affiche Classement sur tous les clients");
        PlayTimer();
        Debug.Log("Cache Classement sur tous les clients");
    }
    [ClientRpc]
    public void TeleporterClientRpc()
    {
        foreach (var client in GameObject.FindGameObjectsWithTag("Player"))
        {
            client.GetComponent<TeleporterJeu>().Teleporter(indexNiveau);
        }
        indexNiveau++;
    }
    [ClientRpc]
    private void ActiverJoueursClientRpc()
    {
        foreach(var client in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (IsOwner)
            {
                client.GetComponent<Jump>().enabled = true;
                client.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
