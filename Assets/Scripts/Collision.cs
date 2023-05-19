using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Fait par: Guillaume Flamand
public class Collision : MonoBehaviour
{
    // Gestionnaire
    [SerializeField] private GestionJeuSolo gestionnaireJeu;
    // Layer
    private static int zoneCollanteLayer = 6;
    private static int zoneAcideLayer = 7;
    private static int trouLayer = 8;
    private static int ondeLayer = 14;
    // Dissolution
    private bool seDissout;
    private bool seConsolide;
    private float alpha = -1.1f;
    private Material material;
    // Sons
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource finNiveauSFX;
    [SerializeField] public AudioSource respawnSFX;
    // Attributs Joueur
    private Rigidbody _rigidbody;
    private Jump jumpComponent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        jumpComponent = GetComponent<Jump>();
    }
    
    private const int layerBouleDeFeu = 9;
    
    void Update()
    {
        if (seDissout)
        {
            alpha += Time.deltaTime;
            material.SetFloat("_Alpha", alpha);
            jumpComponent.enabled = false;
            if (alpha >= 1f)    
            {
                seDissout = false;
                seConsolide = true;
                
                gestionnaireJeu.Ressusciter();
                
                respawnSFX.Play();
            }
        }
        if (seConsolide)
        {
            jumpComponent.enabled = false;
            material.SetColor("_DissolveColor", material.GetColor("_Color"));
            alpha -= Time.deltaTime;
            material.SetFloat("_Alpha", alpha);
            if (alpha <= -1.1f)
            {
                seConsolide = false;
                alpha = -1.1f;
                material.SetFloat("_Alpha", alpha);
                jumpComponent.enabled = true;
            }
        }
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        int collidedLayer = collision.contacts[0].otherCollider.gameObject.layer;
        if (collidedLayer == zoneCollanteLayer)
        {
            DesactiverAcceleration();
        }
        else if (collidedLayer == zoneAcideLayer || collidedLayer == 14)
        {
            deathSFX.Play();
            material.SetColor("_DissolveColor", material.GetColor("_AcidDissolveColor"));
            DesactiverAcceleration();
            Détruire();
        }
        else if (collidedLayer == trouLayer)
        {
            
            finNiveauSFX.Play();
            Sauvegarde.CréerSauvegarde(TimeSpan.FromSeconds(Timer.timeRemaining).ToString(@"mm\:ss\:ff"));
            // gestionnaireJeu.ReinitialiserCompteurSaut();
            jumpComponent.enabled = true;
            material.SetColor("_DissolveColor", Color.red);
            DesactiverAcceleration();  
            
            if (GestionJeuSolo.niveauActuel == 10)
            {
                Debug.Log("entrer dans trou");
                GameObject.Find("Generateur").GetComponent<NiveauProcédural>().boutonRecommencer.GetComponentInParent<Canvas>()
                    .enabled = true;
                GameObject.Find("Generateur").GetComponent<NiveauProcédural>().boutonRecommencer.enabled = true;
                GameObject.Find("Generateur").GetComponent<NiveauProcédural>().fond.enabled = true;
            }
            else
            {
                gestionnaireJeu.ActiverMenuArriverTrou(true);
                gestionnaireJeu.ActiverJoueur(false);
            }
            //isDissolving = true;
           
        }
        else if (collidedLayer == layerBouleDeFeu)
        {
            deathSFX.Play();
            material.SetColor("_DissolveColor", material.GetColor("_FireDissolveColor"));
            DesactiverAcceleration();
            Détruire();
        }
        else if (collidedLayer == ondeLayer)
        {
            Vector3 force = collision.transform.parent.rotation.eulerAngles * 2;
            _rigidbody.AddRelativeForce(force);
            Debug.Log(force);
        }
    }
    private void OnCollisionExit(UnityEngine.Collision other)
    {
        transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
    //private void Ressusciter()
    //{
    //    respawnSFX.Play();
    //    gestionnaireJeu.Ressusciter(gestionnaireJeu.index);
    //}
    public void CollisionLaser()
    {
        deathSFX.Play();
        ChangerCouleurApparition("_LaserDissolveColor");
        Détruire(); 
    }
    private void Détruire()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        seDissout = true;
    }
    private void DesactiverAcceleration()
    {
        transform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
    private void ChangerCouleurApparition(string couleur) => material.SetColor("_DissolveColor", material.GetColor(couleur));
}