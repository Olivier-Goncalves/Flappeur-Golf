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
    // Menu Retour
    [SerializeField] private Button boutonRetour;
    // Menu Début
    [SerializeField] private Button boutonCommencer;
    // Menu Arrivé
    [SerializeField] private Canvas canvaArriver;
    // Menu Timer
    [SerializeField] private Image fondTimer;
    [SerializeField] private TMP_Text timer;
    // Menu Classement
    [SerializeField] private Canvas canvasClassement;
    [SerializeField] private TMP_Text classementTexte;
    private Dictionary<int, int> pointsJoueurs;
    // Autres
    private static string[] couleurs = new[]
    {
        "bleu", "noir", "rouge", "vert", "cyan", "magenta", "jaune", "gris",
        "blanc", "orange"
    };


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
        timeLeft = new NetworkVariable<float>(5);
        boutonRetour.onClick.AddListener(GenererSceneRetour);
        boutonCommencer.onClick.AddListener(() =>
        {
            boutonCommencer.gameObject.GetComponentInParent<Canvas>().enabled = false;
            CommencerPartie();
        });
    }


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
                        joueurMinuteur(false);
                    }
                    else
                    {
                        AfficherClassementClientRpc(false,classementTexte.text);
                        joueurMinuteur(true);
                    }
                }
            }
            
            timer.text = Mathf.RoundToInt(timeLeft.Value).ToString();
            
        }
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
    private void CommencerPartie()
    {
        // gameOn = true;
        nbJoueurs = NetworkManager.ConnectedClientsList.Count;
        CommencerNiveau();
    }
    
    private void CommencerNiveau()
    {
        joueurMinuteur(true);
        TeleporterClientRpc(indexNiveau);
    }
    
    private void joueurMinuteur(bool estActif)
    {
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
            // Classe les joueurs selon leur position et le converti en liste
            var listeJoueurEnOrdreSelonPosition = pointsJoueurs.OrderBy(joueur => joueur.Value).ToList();
            // Prend la liste des 
            var joueurIdEnOrdre = (from joueur in listeJoueurEnOrdreSelonPosition select joueur.Key).Distinct().ToList();

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
