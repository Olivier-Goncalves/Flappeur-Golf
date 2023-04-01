using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Collections;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode.Components;
using Unity.Services.Authentication;

// Fait par: Louis-F�lix Cl�ment
public class GestionMenuMultijoueur : NetworkBehaviour
{
    [SerializeField] private Button boutonRetour;
    [SerializeField] private Button boutonCommencer;
    [SerializeField] private GameObject joueur;
    [SerializeField] private GestionJeuMultijoueur gestionnaireJeu;
    public int index { get; set; }

    private void Awake()
    {
        boutonRetour.onClick.AddListener(GenererSceneRetour);
        boutonCommencer.onClick.AddListener(() =>
        {
            gestionnaireJeu.TeleporterClientRpc();
            boutonCommencer.gameObject.GetComponentInParent<Canvas>().enabled = false;
            gestionnaireJeu.CommencerNiveau();
        });
    }
    
    private void GenererSceneRetour()
    {
        SceneManager.LoadScene("MenuAccueil");
        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Cursor.visible = true;
    }
    public void AfficherPositionArrivée(int positionArrivée)
    {
        Debug.Log("AfficherPositionArrivée");
    }
    public void JouerDécompte()
    {
        Debug.Log("Joue le décompte");
    }
}
