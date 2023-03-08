using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Collections;
using Unity.Services.Authentication;

// Fait par: Louis-Félix Clément
public class GestionMenuMultijoueur : NetworkBehaviour
{
    [SerializeField] private Button boutonRetour;

    private void Awake()
    {
        boutonRetour.onClick.AddListener(GénérerScèneRetour);   
    }

    private void GénérerScèneRetour()
    {
        SceneManager.LoadScene("MenuAccueil");
        if (IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        Cursor.visible = true;
    }
}
