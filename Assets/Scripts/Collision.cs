using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Fait par: Guillaume Flamand
public class Collision : MonoBehaviour
{
    [SerializeField] private GestionJeuSolo gestionnaireJeu;
    private static int StickyZoneLayer = 6;
    
    private static int AcidZoneLayer = 7;
    private static int TrouLayer = 8;
    private static int ondeLayer = 14;
    private bool isDissolving = false;
    private bool isSolving = false;
    private float alpha = -1.1f;
    private Material material;
    [SerializeField] private Vector3 respawn;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource finNiveauSFX;
    [SerializeField] public AudioSource respawnSFX;
    private Transform transformComp;
    private Rigidbody _rigidbody;
    private Jump jumpComponent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        transformComp = GetComponent < Transform>();
        jumpComponent = GetComponent<Jump>();
    }
    
    private const int layerBouleDeFeu = 9;
    
    void Update()
    {
        if (isDissolving)
        {
            alpha += Time.deltaTime;
            material.SetFloat("_Alpha", alpha);
            jumpComponent.enabled = false;
            if (alpha >= 1f)    
            {
                isDissolving = false;
                isSolving = true;
                if (GestionJeuSolo.estNiveauAleatoire)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().useGravity = true;
                    Jump.nbSauts = 0;
                    transform.position = GameObject.Find("Spawn").transform.position;
                    transform.gameObject.SetActive(true);
                    GetComponent<Jump>().enabled = true;
                    GetComponent<MouseControl>().enabled = true;
                    GetComponentInChildren<Camera>().enabled = true;
                }
                else
                {
                    gestionnaireJeu.Ressusciter(gestionnaireJeu.index);
                }
                
                respawnSFX.Play();
            }
        }
        if (isSolving)
        {
            jumpComponent.enabled = false;
            material.SetColor("_DissolveColor", material.GetColor("_Color"));
            alpha -= Time.deltaTime;
            material.SetFloat("_Alpha", alpha);
            if (alpha <= -1.1f)
            {
                isSolving = false;
                alpha = -1.1f;
                material.SetFloat("_Alpha", alpha);
                jumpComponent.enabled = true;
            }
        }
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        int collidedLayer = collision.contacts[0].otherCollider.gameObject.layer;
        if (collidedLayer == StickyZoneLayer)
        {
            DesactiverAcceleration();
        }
        else if (collidedLayer == AcidZoneLayer)
        {
            deathSFX.Play();
            material.SetColor("_DissolveColor", material.GetColor("_AcidDissolveColor"));
            DesactiverAcceleration();
            Détruire();
        }
        else if (collidedLayer == TrouLayer)
        {
            finNiveauSFX.Play();
            Sauvegarde.CréerSauvegarde(TimeSpan.FromSeconds(Timer.timeRemaining).ToString(@"mm\:ss\:ff"));
            gestionnaireJeu.ReinitialiserCompteurSaut();
            jumpComponent.enabled = true;
            material.SetColor("_DissolveColor", Color.red);
            DesactiverAcceleration();
            //isDissolving = true;
            gestionnaireJeu.ActiverMenuArriverTrou(true);
            gestionnaireJeu.ActiverJoueur(false);
            
          
            
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
        isDissolving = true;
    }
    private void DesactiverAcceleration()
    {
        transform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }
    private void ChangerCouleurApparition(string couleur) => material.SetColor("_DissolveColor", material.GetColor(couleur));
}