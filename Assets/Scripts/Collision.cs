using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Fait par: Guillaume Flamand
public class Collision : MonoBehaviour
{
    private const int StickyZoneLayer = 6;

    private const int AcidZoneLayer = 7;

    private const int TrouLayer = 8;

    private const int ondeLayer = 14;

    private bool isDissolving = false;
    private bool isSolving = false;

    private float alpha = -1.1f;

    private Material material;

    private Jump jumpComponent;

    [SerializeField] private Vector3 respawn;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource finNiveauSFX;
    [SerializeField] private AudioSource respawnSFX;

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        jumpComponent = GetComponent<Jump>();
    }
    
    private const int layerBouleDeFeu = 9;
    
    void Update()
    {
        if (isDissolving)
        {
            alpha += Time.deltaTime;
            material.SetFloat("_Alpha", alpha);
            if (alpha >= 1f)
            {
                isDissolving = false;
                Ressusciter();
            }
        }
        if (isSolving)
        {
            ChangerCouleurApparition("_Color");

            alpha -= Time.deltaTime;
            material.SetFloat("_Alpha", alpha);
            if (alpha <= -1.1f)
            {
                isSolving = false;
                alpha = -1.1f;
                material.SetFloat("_Alpha", alpha);
            }
        }
    }
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        switch (collision.contacts[0].otherCollider.gameObject.layer)
        {
            case StickyZoneLayer:
                transform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                break;
            
            case AcidZoneLayer:
                deathSFX.Play();
                ChangerCouleurApparition("_AcidDissolveColor");
                Détruire();
                break;
            
            case TrouLayer:
                // ICI CA NE SE FAIT JAMAIS APPELLER PARCE QUE QUAND ON PREND LE TROU CA TELEPORTE TOUT DE SUITE
                finNiveauSFX.Play();
                ChangerCouleurApparition("_TrouDissolveColor");
                Détruire();
                break;
            
            case layerBouleDeFeu:
                deathSFX.Play();
                ChangerCouleurApparition("_FireDissolveColor");
                Détruire();
                break;
            
            case ondeLayer:
                Vector3 force = collision.transform.rotation.eulerAngles / 2;
                _rigidbody.AddRelativeForce(force);
                break;
        }
    }
    private void OnCollisionExit(UnityEngine.Collision other)
    {
        transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    public void CollisionLaser()
    {
        deathSFX.Play();
        ChangerCouleurApparition("_LaserDissolveColor");
        Détruire();
    }
    private void Ressusciter()
    {
        respawnSFX.Play();
        transform.position = respawn;
        transform.rotation = Quaternion.Euler(0, -90, 0);
        isSolving = true;
        jumpComponent.enabled = true;
    }

    private void Détruire()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        isDissolving = true;
        jumpComponent.enabled = false; 
    }
    private void ChangerCouleurApparition(string couleur) => material.SetColor("_DissolveColor", material.GetColor(couleur));
}
