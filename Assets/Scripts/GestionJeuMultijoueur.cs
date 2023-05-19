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
    public List<Transform> pointsRessuscitement;
    private int positionArrivé;
    private int indexNiveau;
    private int nbJoueurs;
    private bool chronometreActif;
    private NetworkVariable<float> tempsRestant;
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
        tempsRestant = new NetworkVariable<float>(5);
        boutonRetour.onClick.AddListener(GenererSceneRetour);
        boutonCommencer.onClick.AddListener(() =>
        {
            boutonCommencer.gameObject.GetComponentInParent<Canvas>().enabled = false;
            CommencerPartie();
        });
    }


    private void Update()
    {
        if (chronometreActif)
        {
            if (IsHost)
            {
                tempsRestant.Value -= Time.deltaTime;
                if(tempsRestant.Value <= 0)
                {
                    tempsRestant.Value = 5;
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
            
            timer.text = Mathf.RoundToInt(tempsRestant.Value).ToString();
            
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
        chronometreActif = estActif;
    }
    
    [ClientRpc]
    private void AfficherTimerClientRpc(bool afficher)
    {
        timer.enabled = afficher;
        fondTimer.enabled = afficher;
        chronometreActif = true;
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
            // Prend la liste ordonné selon les positions et prend les ids des joueurs pour les mettre dans une liste
            var joueurIdEnOrdre = (from joueur in listeJoueurEnOrdreSelonPosition select joueur.Key).ToList();
            
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < nbJoueurs; i++)
            {
                var indexJoueur = joueurIdEnOrdre.Count - 1 - i;
                sb.Append($"points: {pointsJoueurs[joueurIdEnOrdre[indexJoueur]]} -- Joueur {couleurs[joueurIdEnOrdre[indexJoueur]]}\n");
            }

            classementTexte.text = sb.ToString();
            if (indexNiveau < pointsRessuscitement.Count-1)
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
        chronometreActif = actif;
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
        // Chaque client doit faire la recherche des autres joueurs puisqu'il peut en avoir de nouveaux
        var objetsAvecTag = GameObject.FindGameObjectsWithTag("Player");
        // Nous ne savons pas lequel est notre joueur en tant que tel puisque tous les clients en ont un différent
        foreach(var client in objetsAvecTag)
        {
            client.GetComponent<ParametreJoueur>().ActiverJoueur(estActif);
        }
    }
    
    public void Ressusciter(Transform joueur)
    {
        joueur.position = pointsRessuscitement[indexNiveau].position;
    }
}
