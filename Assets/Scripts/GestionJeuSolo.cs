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
    [SerializeField]private Transform[] spawns;
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
    public bool pause { get; set; }
    public bool gameOn {  get; private set; }
    private int[,] CoupsParTrou;

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
            {30,35,40 },
            {65,70,75 },
            {60,65,70 },
            {60,65,70 },
            {65,70,75 }
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
                pause = true;
                Cursor.visible = true;
            }
            else
            {
                canvasMenuPause.enabled = false;
                pause = false;
                joueur.GetComponent<Jump>().enabled = true;
                //joueur.GetComponent<MouseControl>().enabled = true;   
            }
        }
        if (!gameOn)
        {
            canvasMenuPause.enabled = false;
            AfficherCoupsParTrou(false);
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
        if (index < spawns.Length)
        {
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
        ChangerNiveau();
        ActiverMenuArriverTrou(false);
        ActiverJoueur(true);
        gameOn = true;
    }
    private void clickBoutonRetourPartie()
    {
        ActiverJoueur(true);
        canvasMenuPause.enabled = false;
        pause = false;
        gameOn = true;
    }
    public void Ressusciter(int indexPosition)
    {
        ReinitialiserCompteurSaut();
        index = indexPosition;
        joueur.transform.position = spawns[indexPosition - 1].position;
        ActiverJoueur(true);
        gameOn = true;
        pause = false;
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
        joueur.GetComponent<Jump>().nbSauts = 0;
    }

}
