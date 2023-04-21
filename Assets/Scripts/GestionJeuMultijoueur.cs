using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine.Rendering;


public class GestionJeuMultijoueur : NetworkBehaviour
{
    // Variables Logique jeu
    public List<Transform> spawns;
    private int positionArrivé = 0;
    private int indexNiveau = 0;
    private int nbJoueurs;
    private bool timerOn = false;
    private NetworkVariable<float> timeLeft;
    private bool montrerClassement = false;
    // Variables Menus
    [SerializeField] private Button boutonRetour;
    [SerializeField] private Button boutonCommencer;
    [SerializeField] private Canvas canvaArriver;
    [SerializeField] private Image fondTimer;
    [SerializeField] private Image fondAffichagePosition;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text affichagePosition;
    [SerializeField] private Canvas canvasClassement;
    [SerializeField] private TMP_Text classementTexte;

    private Dictionary<int, int> pointsJoueurs;
    // [SerializeField]
    // private AudioSource musique;
    public static string[] couleurs = new[]
    {
        "bleu", "noir", "rouge", "vert", "cyan", "magenta", "jaune", "gris",
        "blanc", "orange"
    };
    private NetworkList<int> classement;
    private NetworkList<int> points;
    private bool gameOn = false;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // classement = new NetworkList<int>(new int[]{1,2});
        // classement.Add(1);
        // classement.OnListChanged += (previous) => 
            // {Debug.Log($"La liste était [{previous.Value}] et maintenant elle est [{classement[previous.Index]}]"); };
    }

    // ------------------------------------------ DEBUT SECTION MENUS ---------------------------------------------------- //
    private void Awake()
    {
        pointsJoueurs = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 },
            { 9, 0 },
        };
        

        classement = new NetworkList<int>(new int[10]);
        points = new NetworkList<int>(new int[10]);
        timeLeft = new NetworkVariable<float>(5);
        boutonRetour.onClick.AddListener(GenererSceneRetour);
        boutonCommencer.onClick.AddListener(() =>
        {
            boutonCommencer.gameObject.GetComponentInParent<Canvas>().enabled = false;
            CommencerPartie();
        });
        // musique.Play();
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
    
    // --------------------------------------------- FIN SECTION MENUS ------------------------------------------------- // 
    private void Update()
    {
        // if (gameOn && IsHost)
        // {
        //     classement.Add(2);
        //     var attendre = new WaitForSeconds(5f);
        //     
        // }
        if (timerOn && IsHost)
        {
            timeLeft.Value -= Time.deltaTime;
            timer.text = Mathf.RoundToInt(timeLeft.Value).ToString();
            if(timeLeft.Value <= 0)
            {
                // timerOn = false;
                timeLeft.Value = 5;
                if (!montrerClassement)
                {
                    AfficherTimerClientRpc(false);
                    timer.enabled = false;
                    fondTimer.enabled = false;
                    ActiverJoueursClientRpc(true);
                    timerOn = false;
                }
                else
                {
                    AfficherClassementClientRpc(false,classementTexte.text);
                    PlayTimer();
                }
            }
        }
        // Debug.Log("timer: " + timerOn);
        // Debug.Log("classement: " + montrerClassement);
        // Debug.Log(timeLeft.Value);
        // Debug.Log($"classement[0]: {classement[9]}, classement[1]: {classement[8]} ");
        // Debug.Log($"[0, {pointsJoueurs[0]}], [1, {pointsJoueurs[1]}]");
    }
    public void CommencerPartie()
    {
        gameOn = true;
        nbJoueurs = NetworkManager.ConnectedClientsList.Count;
        CommencerNiveau();
    }
    
    public void CommencerNiveau()
    {
        PlayTimer();
        TeleporterClientRpc(indexNiveau);
    }
    
    private void PlayTimer()
    {
        // Debug.Log("Pars le Timer de 5 secondes");
        
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
    
    public void ArriverTrou(int networkId)
    {
        GererArriverTrouServerRpc(networkId);
        foreach(var client in GameObject.FindGameObjectsWithTag("Player"))
        {
            client.GetComponent<ParametreJoueur>().ActiverJoueur(false);
        }
    }

    private void AjusterClassement(int nouvellePosition, int joueurId)
    {
        // classement[nouvellePosition] = joueurId;
        points[joueurId] += nbJoueurs - nouvellePosition;
        pointsJoueurs[joueurId] += nbJoueurs - nouvellePosition;
        Debug.Log(points[joueurId]);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void GererArriverTrouServerRpc(int networkId)
    {
        AjusterClassement(positionArrivé, networkId);
        AjusterPositionJoueurClientRpc(positionArrivé + 1);
        // timerOn = false;
        if (positionArrivé == nbJoueurs)
        {
            
            var mySortedList = pointsJoueurs.OrderBy(d => d.Value).ToList();
            var joueurIdEnOrdre = (from test in mySortedList select test.Key).Distinct().ToList();
            classement = new NetworkList<int>(joueurIdEnOrdre.ToArray());
            foreach (var element in classement)
            {
                Debug.Log("element dans classement: "+element);
            }
            classementTexte.text = $"points: {pointsJoueurs[joueurIdEnOrdre[9]]} -- Joueur {couleurs[joueurIdEnOrdre[9]]}\n" +
                                   $"points: {pointsJoueurs[joueurIdEnOrdre[8]]} -- Joueur {couleurs[joueurIdEnOrdre[8]]}\n" +
                                   $"{couleurs[2]}\n" +
                                   $"{couleurs[3]}\n" +
                                   $"{couleurs[4]}\n" +
                                   $"{couleurs[5]}\n" +
                                   $"{couleurs[6]}\n" +
                                   $"{couleurs[7]}\n" +
                                   $"{couleurs[8]}\n" +
                                   $"{couleurs[9]}\n";
                                   // // Debug.Log("Le if est appellé");
                                   AfficherClassementClientRpc(true, classementTexte.text);
            indexNiveau++;
            CommencerNiveau();
            AjusterPositionJoueurClientRpc(0);
        }
    }
    [ClientRpc]
    private void AjusterPositionJoueurClientRpc(int position)
    {
        positionArrivé = position;
        // Debug.Log("position: " + positionArrivé);
    }
    
    [ClientRpc]
    private void AfficherClassementClientRpc(bool actif, string texteClassement)
    {
        // Debug.Log("Affiche Classement sur tous les clients");
        // var mySortedList = pointsJoueurs.Value.OrderBy(d => d.Value).ToList();
        // var joueurIdEnOrdre = (from test in mySortedList select test.Key).Distinct().ToList();
        // classement = new NetworkList<int>(joueurIdEnOrdre);
        // var valeur = (from test in mySortedList select test.Value).Distinct().ToList();
        // foreach (var pair in mySortedList)
        // {
        //     Debug.Log(pair);
        // }
        // Debug.Log("-------------------------------------------------------------");
        // foreach (var element in valeur)
        // {
        //     Debug.Log("valeur: " + element);
        // }
        // Debug.Log("---------------------------------------------------------------------");
        // foreach (var element in joueurIdEnOrdre)
        // {
        //     Debug.Log("joueurId: " + element);
        // }
        
        // classementTexte.text = $"points: {pointsJoueurs.Value[joueurIdEnOrdre[9]]} -- Joueur {couleurs[joueurIdEnOrdre[9]]}\n" +
        //                        $"points: {pointsJoueurs.Value[joueurIdEnOrdre[8]]} -- Joueur {couleurs[joueurIdEnOrdre[8]]}\n" +
        //                        $"{couleurs[2]}\n" +
        //                        $"{couleurs[3]}\n" +
        //                        $"{couleurs[4]}\n" +
        //                        $"{couleurs[5]}\n" +
        //                        $"{couleurs[6]}\n" +
        //                        $"{couleurs[7]}\n" +
        //                        $"{couleurs[8]}\n" +
        //                        $"{couleurs[9]}\n";
        classementTexte.text = texteClassement;
        montrerClassement = actif;
        timerOn = actif;
        canvasClassement.enabled = actif;
        // Debug.Log("Cache Classement sur tous les clients");
        
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
