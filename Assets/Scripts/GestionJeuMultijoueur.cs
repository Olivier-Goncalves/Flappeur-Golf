using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GestionJeuMultijoueur : NetworkBehaviour
{
    // Variables Logique jeu
    public List<Transform> spawns;
    private int positionArrivé;
    private int indexNiveau;
    private int nbJoueurs;
    private bool timerOn;
    private NetworkVariable<float> timeLeft;

    private bool montrerClassement;
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
    private static string[] couleurs = new[]
    {
        "bleu", "noir", "rouge", "vert", "cyan", "magenta", "jaune", "gris",
        "blanc", "orange"
    };
    private bool gameOn;

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
        

        // classement = new NetworkList<int>(new int[10]);
        // points = new NetworkList<int>(new int[10]);
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
        if (timerOn)
        {
            if (IsHost)
            {
                timeLeft.Value -= Time.deltaTime;
                if(timeLeft.Value <= 0)
                {
                    timeLeft.Value = 5;
                    if (!montrerClassement)
                    {
                        PlayTimer(false);
                    }
                    else
                    {
                        AfficherClassementClientRpc(false,classementTexte.text);
                        PlayTimer(true);
                    }
                }
            }
            
            timer.text = Mathf.RoundToInt(timeLeft.Value).ToString();
            
        }
    }
    private void CommencerPartie()
    {
        gameOn = true;
        nbJoueurs = NetworkManager.ConnectedClientsList.Count;
        CommencerNiveau();
    }
    
    private void CommencerNiveau()
    {
        PlayTimer(true);
        TeleporterClientRpc(indexNiveau);
    }
    
    private void PlayTimer(bool estActif)
    {
        // Debug.Log("Pars le Timer de 5 secondes");
        
        timer.enabled = estActif;
        fondTimer.enabled = estActif;
        ActiverJoueursClientRpc(!estActif);
        AfficherTimerClientRpc(estActif);
        timerOn = estActif;
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

    
    [ServerRpc(RequireOwnership = false)]
    private void GererArriverTrouServerRpc(int networkId)
    {
        AjusterClassement(positionArrivé, networkId);
        AjusterPositionJoueurClientRpc(positionArrivé + 1);
        if (positionArrivé == nbJoueurs)
        {
            
            var mySortedList = pointsJoueurs.OrderBy(d => d.Value).ToList();
            var joueurIdEnOrdre = (from test in mySortedList select test.Key).Distinct().ToList();
            //foreach (var element in joueurIdEnOrdre)
            //{
            //    Debug.Log("element dans classement: "+element);
            //}

            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < nbJoueurs; i++)
            {
                var indexJoueur = joueurIdEnOrdre.Count - 1 - i;
                sb.Append($"points: {pointsJoueurs[joueurIdEnOrdre[indexJoueur]]} -- Joueur {couleurs[joueurIdEnOrdre[indexJoueur]]}\n");
            }

                classementTexte.text = sb.ToString();
            if (indexNiveau < spawns.Count-1)
            {
                AfficherClassementClientRpc(true, classementTexte.text);
                indexNiveau++;
                CommencerNiveau();
                AjusterPositionJoueurClientRpc(0);
            }
            else
            {
                FinPartieClientRpc();
            }
        }
    }
    [ClientRpc]
    private void FinPartieClientRpc()
    {
        Debug.Log("Fin Partie");
        classementTexte.enabled = true;
        canvasClassement.enabled = true;
        boutonRetour.enabled = true;
        boutonCommencer.enabled = false;
        canvaArriver.enabled = true;
    }
    private void AjusterClassement(int nouvellePosition, int joueurId)
    {
        pointsJoueurs[joueurId] += nbJoueurs - nouvellePosition;
    }
    [ClientRpc]
    private void AjusterPositionJoueurClientRpc(int position)
    {
        positionArrivé = position;
    }
    
    [ClientRpc]
    private void AfficherClassementClientRpc(bool actif, string texteClassement)
    {
        classementTexte.text = texteClassement;
        montrerClassement = actif;
        timerOn = actif;
        canvasClassement.enabled = actif;
    }
    
    [ClientRpc]
    private void TeleporterClientRpc(int index)
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
        var objetsAvecTag = GameObject.FindGameObjectsWithTag("Player");
        foreach(var client in objetsAvecTag)
        {
            client.GetComponent<ParametreJoueur>().ActiverJoueur(estActif);
        }
    }
    
    public void Ressusciter(Transform joueur)
    {
        joueur.position = spawns[indexNiveau].position;
    }
}
