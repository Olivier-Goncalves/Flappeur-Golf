using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Fait par: Louis-Félix Clément
public class ParametreJoueur : MonoBehaviour
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
}
