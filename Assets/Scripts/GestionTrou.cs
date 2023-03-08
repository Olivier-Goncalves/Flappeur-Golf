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
    private Canvas canvas;
    private GameObject camera;
    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        camera = new GameObject("Camera");
        camera.AddComponent<Camera>();
        camera.transform.position = new Vector3(0, 40, 0);
        camera.GetComponent<Camera>().enabled = false;
        boutonMenu.onClick.AddListener(clickBoutonMenu);
        prochainNiveau.onClick.AddListener(clickBoutonProchainNiveau);
        
    }

    private void clickBoutonMenu()
    {
        SceneManager.LoadScene("Solo");
    }
    private void clickBoutonProchainNiveau()
    {
        SceneManager.LoadScene("Trou1");
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.gameObject.layer==10){
        Debug.Log("Trou Atteint");
        textFélicitation.enabled = true;
        canvas.enabled = true;
        collision.gameObject.GetComponent<Jump>().enabled = false;
        collision.gameObject.GetComponent<MouseControl>().enabled = false;
        collision.gameObject.GetComponent<MouseControl>().enabled = false;
        
        collision.gameObject.GetComponentInChildren<Camera>().enabled = false;
        camera.GetComponent<Camera>().enabled = true;
        }
    }
    
    
    
}
