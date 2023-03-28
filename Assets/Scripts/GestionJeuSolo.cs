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
    private bool pause = false;
    private bool gameOn = false;

    private void Awake()
    {
        index = 1;
        boutonMenuTrou.onClick.AddListener(clickBoutonMenu);
        prochainNiveauTrou.onClick.AddListener(clickBoutonProchainNiveau);
        btnRetourPartie.onClick.AddListener(clickBoutonRetourPartie);
        btnMenuPause.onClick.AddListener(clickBoutonMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!pause && gameOn)
            {
                canvasMenuPause.enabled = true;
                joueur.GetComponent<Jump>().enabled = false;
                pause = true;
                
            }
            else
            {
                canvasMenuPause.enabled = false;
                pause = false;
            }
        }
        if (!gameOn)
        {
            canvasMenuPause.enabled = false;
        }
    }
    public void ChangerNiveau()
    {
        if(index < spawns.Length)
            joueur.transform.position = spawns[index++].position;
        ActiverJoueur();
        gameOn = true;
    }
    private void clickBoutonMenu()
    {
        canvasMenuPrincipal.enabled = true;
        DesactiverMenuArriverTrou();
        DesactiverJoueur();
        gameOn = false;
        
    }
    private void clickBoutonProchainNiveau()
    {
        ChangerNiveau();
        ActiverJoueur();
        DesactiverMenuArriverTrou();
        gameOn = true;
    }
    private void clickBoutonRetourPartie()
    {
        ActiverJoueur();
        canvasMenuPause.enabled = false;
        gameOn = true;
    }
    public void Ressusciter(int indexPosition)
    {
        index = indexPosition;
        joueur.transform.position = spawns[indexPosition - 1].position;
        ActiverJoueur();
        gameOn = true;
    }
    private void DesactiverMenuArriverTrou()
    {
        textFélicitation.enabled = false;
        boutonMenuTrou.enabled = false;
        prochainNiveauTrou.enabled = false;
        canvasMenuArrive.enabled = false;
    }
    private void ActiverJoueur()
    {
        joueur.SetActive(true);
        joueur.GetComponent<Jump>().enabled = true;
        joueur.GetComponent<MouseControl>().enabled = true;
        joueur.GetComponentInChildren<Camera>().enabled = true;
    }
    private void DesactiverJoueur()
    {
        joueur.SetActive(false);
        joueur.gameObject.GetComponent<Jump>().enabled = false;
        joueur.gameObject.GetComponent<MouseControl>().enabled = false;
        joueur.gameObject.GetComponentInChildren<Camera>().enabled = false;
    }
}
