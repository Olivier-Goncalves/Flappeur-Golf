using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;


public class GestionJeuMultijoueur : NetworkBehaviour
{
    // Variables Logique jeu
    public Transform[] spawns;
    private int positionArrivé = 0;
    private int indexNiveau = 0;
    private int nbJoueurs;
    private bool timerOn = false;
    private float timeLeft = 5;
    private bool montrerClassement = false;
    private bool décompte = false;
    // Variables Menus
    [SerializeField] private Button boutonRetour;
    [SerializeField] private Button boutonCommencer;
    [SerializeField] private Canvas canvaArriver;
    [SerializeField] private Image fondTimer;
    [SerializeField] private Image fondAffichagePosition;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text affichagePosition;
    [SerializeField] private Canvas canvasClassement;


    // ------------------------------------------ DEBUT SECTION MENUS ---------------------------------------------------- //
    private void Awake()
    {
        boutonRetour.onClick.AddListener(GenererSceneRetour);
        boutonCommencer.onClick.AddListener(() =>
        {
            boutonCommencer.gameObject.GetComponentInParent<Canvas>().enabled = false;
            CommencerPartie();
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
    public void JouerDécompte(bool jouer)
    {
        Debug.Log("Joue le décompte");
        timer.enabled = jouer;
        fondTimer.enabled = jouer;
    }
    // --------------------------------------------- SECTION MENUS ------------------------------------------------- // 
    private void Update()
    {
        if (timerOn)
        {
            timeLeft -= Time.deltaTime;
            timer.text = Mathf.RoundToInt(timeLeft).ToString();
            if(timeLeft <= 0)
            {
                timerOn = false;
                timeLeft = 5;
                if (IsHost && !montrerClassement)
                {
                    timer.enabled = false;
                    fondTimer.enabled = false;
                    ActiverJoueursClientRpc(true);
                }
                
                if (montrerClassement)
                {
                    canvasClassement.enabled = false;
                    montrerClassement = false;
                    PlayTimer();
                }
            }
        }
    }
    public void CommencerPartie()
    {
        nbJoueurs = NetworkManager.ConnectedClientsList.Count;
        CommencerNiveau();
    }
    
    public void CommencerNiveau()
    {
        PlayTimer();
        // WaitForSeconds wait = new WaitForSeconds(5);
        TeleporterClientRpc(indexNiveau);
        // Jouer Décompte
        // ActiverJoueursClientRpc(true);
    }
    
    private void PlayTimer()
    {
        Debug.Log("Pars le Timer de 5 secondes");
        timerOn = true;
        timer.enabled = true;
        fondTimer.enabled = true;
        ActiverJoueursClientRpc(false);
        AfficherTimerClientRpc(true);
    }
    [ClientRpc]
    private void AfficherTimerClientRpc(bool afficher)
    {
        timer.enabled = afficher;
        fondTimer.enabled = afficher;
        timerOn = true;
    }
    
    public void ArriverTrou()
    {
        GererArriverTrouServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void GererArriverTrouServerRpc()
    {
        AjusterPositionJoueurClientRpc(positionArrivé + 1);
        if (positionArrivé == nbJoueurs)
        {
            AfficherClassementClientRpc();
            
            indexNiveau++;
            CommencerNiveau();
            AjusterPositionJoueurClientRpc(0);
        }
    }
    [ClientRpc]
    private void AjusterPositionJoueurClientRpc(int ajout)
    {
        positionArrivé = ajout;
        Debug.Log("position: " + positionArrivé);
    }
    
    [ClientRpc]
    private void AfficherClassementClientRpc()
    {
        Debug.Log("Affiche Classement sur tous les clients");
        montrerClassement = true;
        timerOn = true;
        canvasClassement.enabled = true;
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
    private void ActiverJoueursClientRpc(bool estActif)
    {
        foreach(var client in GameObject.FindGameObjectsWithTag("Player"))
        {
            client.GetComponent<ParametreJoueur>().ActiverJoueur(estActif);
        }
    }
    
    public void Ressusciter(Transform joueur)
    {
        joueur.position = spawns[indexNiveau].position;
    }
}
