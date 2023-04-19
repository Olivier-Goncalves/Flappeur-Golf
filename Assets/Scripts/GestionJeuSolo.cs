using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Fait par Olivier Gonçalves
public class GestionJeuSolo : MonoBehaviour
{
    public int index { get; set; }
    [SerializeField] private List<Transform> spawns;
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
    // volume slider
    [SerializeField] private Canvas canvasVolumeSlider;
    public bool pause { get; set; }
    public bool gameOn {  get; private set; }
    private int[,] CoupsParTrou;

    public static int niveauActuel = 1;
    private void Awake()
    {
        gameOn = false;
        index = 1;
        boutonMenuTrou.onClick.AddListener(clickBoutonMenu);
        prochainNiveauTrou.onClick.AddListener(clickBoutonProchainNiveau);
        btnRetourPartie.onClick.AddListener(clickBoutonRetourPartie);
        btnMenuPause.onClick.AddListener(clickBoutonMenu);
        CoupsParTrou = new int[,] 
        {
            {30,35,40},
            {65,70,75},
            {60,65,70},
            {60,65,70},
            {65,70,75}
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause && gameOn)
            {
                canvasMenuPause.enabled = true;
                joueur.GetComponent<Jump>().enabled = false;
                //joueur.GetComponent<MouseControl>().enabled = false;
                ChangerPause(true);
                Cursor.visible = true;
                canvasVolumeSlider.gameObject.SetActive(true);
            }
            else
            {
                canvasMenuPause.enabled = false;
                ChangerPause(false);
                joueur.GetComponent<Jump>().enabled = true;
                //joueur.GetComponent<MouseControl>().enabled = true;   
                canvasVolumeSlider.gameObject.SetActive(false);
            }
        }
        if (!gameOn)
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

    public void AfficherCoupsParTrou(bool actif)
    {
        if (actif)
        { 
            nbCoupsTexte.text = $"{CoupsParTrou[index - 1,0]}         {CoupsParTrou[index - 1, 1]}         {CoupsParTrou[index - 1, 2]}";
        }
        nbCoupsTexte.GetComponentInParent<Canvas>().enabled = actif;
        
    }
    public void ChangerNiveau()
    {
        if (index < spawns.Count)
        {
            niveauActuel++;
            index++;
            Ressusciter(index);
        }

        ActiverJoueur(true);
        gameOn = true;
    }
    private void clickBoutonMenu()
    {
        canvasMenuPrincipal.enabled = true;
        ActiverMenuArriverTrou(false);
        ActiverJoueur(false);
        gameOn = false;
        
    }
    private void clickBoutonProchainNiveau()
    {
        joueur.GetComponent<Jump>().isOnGreen = false;
        ChangerNiveau();
        ActiverMenuArriverTrou(false);
        ActiverJoueur(true);
        gameOn = true;
    }
    private void clickBoutonRetourPartie()
    {
        canvasVolumeSlider.gameObject.SetActive(true);
        ActiverJoueur(true);
        canvasMenuPause.enabled = false;
        ChangerPause(false);
        gameOn = true;
    }
    public void Ressusciter(int indexPosition)
    {
        joueur.GetComponent<Rigidbody>().velocity = Vector3.zero;
        joueur.GetComponent<Rigidbody>().useGravity = true;
        ReinitialiserCompteurSaut();
        index = indexPosition;
        joueur.transform.position = spawns[indexPosition - 1].position;
        ActiverJoueur(true);
        gameOn = true;
        ChangerPause(false);
    }
    public void ActiverMenuArriverTrou(bool actif)
    {
        boutonMenuTrou.enabled = actif;
        prochainNiveauTrou.enabled = actif;
        canvasMenuArrive.enabled = actif;
        joueur.GetComponent<Jump>().enabled = false;
    }
    public void ActiverJoueur(bool actif)
    {
        joueur.SetActive(actif);
        joueur.GetComponent<Jump>().enabled = actif;
        joueur.GetComponent<MouseControl>().enabled = actif;
        joueur.GetComponentInChildren<Camera>().enabled = actif;
    }
    public void ReinitialiserCompteurSaut()
    {
        Jump.nbSauts = 0;
    }
    private void ChangerPause(bool estEnPause)
    {
        pause = estEnPause;
        joueur.GetComponent<MouseControl>().pause = estEnPause;
    }

}
