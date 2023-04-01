using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GestionJeuMultijoueur : NetworkBehaviour
{
    public Transform[] spawns;
    private int positionArriv� = 0;
    private int indexNiveau = 0;
    [SerializeField] GestionMenuMultijoueur gestionnaireMenus;

    public void CommencerNiveau()
    {
        PlayTimer();
        TeleporterClientRpc();
        // Jouer D�compte
        gestionnaireMenus.JouerD�compte();
        ActiverJoueursClientRpc();
    }
    
    private void PlayTimer()
    {
        Debug.Log("Pars le Timer de 5 secondes");
    }

    public void ArriverTrou()
    {
        gestionnaireMenus.AfficherPositionArriv�e(positionArriv�);
        if(positionArriv� + 1 == NetworkManager.ConnectedClientsList.Count)
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
