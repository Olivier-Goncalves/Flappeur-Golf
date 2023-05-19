using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Fait par Olivier Gonçalves
public class GestionJeuSolo : MonoBehaviour
{
    public int indexNiveau { get; set; }
    [SerializeField] private GameObject joueur;
    //Menu Arrive Trou
    [SerializeField] private Canvas canvasMenuArrive;
    [SerializeField] private Button boutonMenuTrou;
    [SerializeField] private Button prochainNiveauTrou;
    [SerializeField] private TMP_Text textFélicitation;
    // Menu Principal
    [SerializeField] private Canvas canvasMenuPrincipal;
    // Menu Pause
    [SerializeField] private Canvas canvasMenuPause;
    [SerializeField] private Button btnRetourPartie;
    [SerializeField] private Button btnMenuPause;
    // Menu Nombre de Sauts
    [SerializeField] private TMP_Text nbCoupsTexte;
    [SerializeField] Canvas canvasNombreSauts;
    // volume slider
    [SerializeField] private Canvas canvasVolumeSlider;
    // Mode de jeu
    public bool pause { get; set; }
    public bool jeuEnCours {  get; private set; }
    // Attributs joueur
    private Saut composantSautJoueur;
    private MouseControl composantControlesJoueur;
    private Rigidbody rbJoueur;
    private Camera cameraJoueur;
    public static int[,] CoupsParTrou = new int[,] 
    {
        {30,35,40},
        {65,70,75},
        {60,65,70},
        {60,65,70},
        {65,70,75},
        {65,70,75},
        {65,70,75},
        {65,70,75},
        {65,70,75},
    };

    public static int niveauActuel = 1;

    private void Awake()
    {
        composantSautJoueur = joueur.GetComponent<Saut>();
        composantControlesJoueur = joueur.GetComponent<MouseControl>();
        rbJoueur = joueur.GetComponent<Rigidbody>();
        cameraJoueur = joueur.GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        if (niveauActuel != 10)
        {
            jeuEnCours = false;
            indexNiveau = 1;
            boutonMenuTrou.onClick.AddListener(clickBoutonMenu);
            prochainNiveauTrou.onClick.AddListener(clickBoutonProchainNiveau);
            btnRetourPartie.onClick.AddListener(clickBoutonRetourPartie);
            btnMenuPause.onClick.AddListener(clickBoutonMenu);
        }
    }

    private void Update()
    {
        if (niveauActuel != 10)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pause && jeuEnCours)
                {
                    canvasMenuPause.enabled = true;
                    composantSautJoueur.enabled = false;
                    ChangerPause(true);
                    Cursor.visible = true;
                    canvasVolumeSlider.gameObject.SetActive(true);
                }
                else
                {
                    canvasMenuPause.enabled = false;
                    ChangerPause(false);
                    composantSautJoueur.enabled = true;
                    canvasVolumeSlider.gameObject.SetActive(false);
                }
            }
            if (!jeuEnCours)
            {
                canvasMenuPause.enabled = false;
                AfficherCoupsParTrou(false);
                canvasVolumeSlider.gameObject.SetActive(false);
            }
            else
            {
                AfficherCoupsParTrou(true);
            }
        }
    }

    public void AfficherCoupsParTrou(bool actif)
    {
        if (actif)
        { 
            nbCoupsTexte.text = $"{CoupsParTrou[indexNiveau - 1,0]}         {CoupsParTrou[indexNiveau - 1, 1]}         {CoupsParTrou[indexNiveau - 1, 2]}";
        }
        canvasNombreSauts.enabled = actif;
        
    }
    public void ChangerNiveau()
    {
        if (indexNiveau < Spawns.spawns.Count)
        {
            niveauActuel++;
            Spawns.spawnActuel = Spawns.spawns[niveauActuel - 1];
            Ressusciter();
        }
        ActiverJoueur(true);
        jeuEnCours = true;
    }
    private void clickBoutonMenu()
    {
        canvasMenuPrincipal.enabled = true;
        ActiverMenuArriverTrou(false);
        ActiverJoueur(false);
        jeuEnCours = false;
    }
    private void clickBoutonProchainNiveau()
    {
        composantSautJoueur.isOnGreen = false;
        ChangerNiveau();
        ActiverMenuArriverTrou(false);
        ActiverJoueur(true);
        jeuEnCours = true;
    }
    private void clickBoutonRetourPartie()
    {
        canvasVolumeSlider.gameObject.SetActive(true);
        ActiverJoueur(true);
        canvasMenuPause.enabled = false;
        ChangerPause(false);
        jeuEnCours = true;
    }
    public void Ressusciter()
    {
        rbJoueur.velocity = Vector3.zero;
        rbJoueur.useGravity = true;
        ReinitialiserCompteurSaut();
        joueur.transform.position = Spawns.spawnActuel;
        ActiverJoueur(true);
        jeuEnCours = true;
        ChangerPause(false);
    }
    public void ActiverMenuArriverTrou(bool actif)
    {
        boutonMenuTrou.enabled = actif;
        prochainNiveauTrou.enabled = actif;
        canvasMenuArrive.enabled = actif;
        composantSautJoueur.enabled = false;
    }
    public void ActiverJoueur(bool actif)
    {
        joueur.SetActive(actif);
        composantSautJoueur.enabled = actif;
        composantControlesJoueur.enabled = actif;
        cameraJoueur.enabled = actif;
    }
    public void ReinitialiserCompteurSaut()
    {
        composantSautJoueur.RéinitialiserCompteurSauts();
    }
    private void ChangerPause(bool estEnPause)
    {
        pause = estEnPause;
        composantControlesJoueur.pause = estEnPause;
    }
}
