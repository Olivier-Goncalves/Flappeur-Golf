using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
// Fait par: Louis-Félix Clément
public class ParametreJoueur : NetworkBehaviour
{
    private Jump jumpComponent;
    private Rigidbody rigidbodyComponent;
    private void Awake()
    {
        jumpComponent = GetComponent<Jump>();
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    public void ActiverJoueur(bool estActif)
    {
        if (IsOwner)
        {
            jumpComponent.enabled = estActif;
            rigidbodyComponent.useGravity = estActif;
        }
    }
}
