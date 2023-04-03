using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.Netcode;
using UnityEngine;

public class GestionJeuMultijoueur : NetworkBehaviour
{
    public Transform[] spawns;
    public int positionArrivé = 0;
    public int indexNiveau = 0;
    [SerializeField] GestionMenuMultijoueur gestionnaireMenus;
    public int nbJoueurs;
   
    
    public void CommencerPartie()
    {
        nbJoueurs = NetworkManager.ConnectedClientsList.Count;
        CommencerNiveau();
    }
    
    public void CommencerNiveau()
    {
        PlayTimer();
        TeleporterClientRpc(indexNiveau);
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
        indexNiveau++;
        AppellerTeleporterServerRpc(indexNiveau);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AppellerTeleporterServerRpc(int index)
    {
        TeleporterClientRpc(index);
        ActiverJoueursClientRpc();
    }
    
    
    [ClientRpc]
    private void AfficherClassementClientRpc()
    {
        Debug.Log("Affiche Classement sur tous les clients");
        PlayTimer();
        Debug.Log("Cache Classement sur tous les clients");
    }
    
    [ClientRpc]
    public void TeleporterClientRpc(int index)
    {
        foreach (var client in GameObject.FindGameObjectsWithTag("Player"))
        {
            client.GetComponent<TeleporterJeu>().Teleporter(index);
        }

        GameObject.Find("GestionnaireJeu").GetComponent<GestionJeuMultijoueur>().indexNiveau = index;
    }
    
    [ClientRpc]
    private void ActiverJoueursClientRpc()
    {
        foreach(var client in GameObject.FindGameObjectsWithTag("Player"))
        {
            client.GetComponent<ParametreJoueur>().ActiverJoueur();
        }
    }
    
    public void Ressusciter(Transform joueur)
    {
        joueur.position = spawns[indexNiveau].position;
    }
}
