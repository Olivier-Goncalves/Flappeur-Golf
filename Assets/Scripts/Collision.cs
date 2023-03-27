using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Fait par: Guillaume Flamand
public class Collision : MonoBehaviour
{
    private static int StickyZoneLayer = 6;

    private static int AcidZoneLayer = 7;

    private static int TrouLayer = 8;

    private static int ondeLayer = 14;

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
            material.SetColor("_DissolveColor", material.GetColor("_Color"));

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
        int collidedLayer = collision.contacts[0].otherCollider.gameObject.layer;

        if (collidedLayer == StickyZoneLayer)
        {
            transform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        else if (collidedLayer == AcidZoneLayer)
        {
            deathSFX.Play();
            material.SetColor("_DissolveColor", material.GetColor("_AcidDissolveColor"));
            Détruire();

        }
        else if (collidedLayer == TrouLayer)
        {
            finNiveauSFX.Play();
            material.SetColor("_DissolveColor", Color.red);
            Détruire();

        }
        else if (collidedLayer == layerBouleDeFeu)
        {
            deathSFX.Play();
            material.SetColor("_DissolveColor", material.GetColor("_FireDissolveColor"));
            Détruire();
        }
        else if (collidedLayer == ondeLayer)
        {
            Vector3 force = collision.transform.rotation.eulerAngles / 2;
            _rigidbody.AddRelativeForce(force);
            Debug.Log(force);
        }
    }
    private void OnCollisionExit(UnityEngine.Collision other)
    {
        transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    public void ToucheLaser()
    {
        deathSFX.Play();
        material.SetColor("_DissolveColor", material.GetColor("_LaserDissolveColor"));
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
        transform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
        isDissolving = true;
        jumpComponent.enabled = false; 
    }
}
