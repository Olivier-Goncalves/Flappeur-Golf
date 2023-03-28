using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Fait par: Olivier Gonçalves
[RequireComponent(typeof(Collider))]
public class GestionTrou : MonoBehaviour
{
    [SerializeField] private TMP_Text textFélicitation;
    [SerializeField] private Button boutonMenu;
    [SerializeField] private Button prochainNiveau;
    [SerializeField] private GestionJeuSolo gestionnaireJeu;
    [SerializeField] private Canvas canvas;
    

   
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.gameObject.layer==10){
            Debug.Log("Trou Atteint"); 
            canvas.enabled = true;
            textFélicitation.enabled = true;
            prochainNiveau.enabled = true;
            boutonMenu.enabled = true;
            collision.gameObject.GetComponent<Jump>().enabled = false;
            collision.gameObject.GetComponent<MouseControl>().enabled = true;
            collision.gameObject.GetComponentInChildren<Camera>().enabled = true;
        }
    }
}
