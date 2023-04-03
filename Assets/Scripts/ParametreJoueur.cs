using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
// Fait par: Louis-Félix Clément
public class ParametreJoueur : NetworkBehaviour
{
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

    public void ActiverJoueur()
    {
        if (IsOwner)
        {
            GetComponent<Jump>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
