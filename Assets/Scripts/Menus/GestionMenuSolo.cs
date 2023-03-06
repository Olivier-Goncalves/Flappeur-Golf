using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionMenuSolo : MonoBehaviour
{
    [SerializeField] private Button[] boutonsTrous = new Button[9];

    private void Awake()
    {
        for (int i = 0; i < boutonsTrous.Length; i++)
        {
            boutonsTrous[i].onClick.AddListener(GenererScene);
        }
    }

    private void GenererScene()
    {
        
            SceneManager.LoadScene("Trou1");
        
    }
}
