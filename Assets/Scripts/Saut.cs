using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TMPro;
using Unity.Netcode;
using UnityEngine;
// Fait par: Louis-Félix Bouvrette
[RequireComponent(typeof(Rigidbody))]
public class Saut : MonoBehaviour
{
    [SerializeField] private AudioSource flapSFX;
    [SerializeField] private Material green;
    public float jumpStrength = 100;
    [SerializeField] TMP_Text texteCompteurSauts;
    private Rigidbody _rigidbody;
    public int nbSauts { get; private set; }
    public bool isOnGreen = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    void Update()
    {
        Vector3 velocity = _rigidbody.velocity;
        if (JoueurSaute() && isOnGreen)
        {
            flapSFX.Play();
            _rigidbody.velocity = velocity / 1.5f - new Vector3(0, 0, velocity.z / 2);
            _rigidbody.AddRelativeForce(new Vector3(0, 0, jumpStrength * 12));
            nbSauts++;
        }
        else if (JoueurSaute())
        {
            flapSFX.Play();
            _rigidbody.velocity = velocity / 1.5f - new Vector3(0, velocity.y / 2, 0);
            _rigidbody.AddRelativeForce(new Vector3(0, jumpStrength * 10, jumpStrength * 12));
            nbSauts++;
        }
        texteCompteurSauts.text = nbSauts.ToString();
    }

    private bool JoueurSaute() => Input.GetKeyUp(KeyCode.Space);
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.GetComponent<Renderer>().material.name == "green (Instance)")
        {
            isOnGreen = true;
        }
    }
    private void OnCollisionExit(UnityEngine.Collision other)
    {
        isOnGreen = false;
    }

    public void RéinitialiserCompteurSauts()
    {
        nbSauts = 0;
    }
}