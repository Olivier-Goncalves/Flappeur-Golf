using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Collections;
using Unity.Services.Authentication;

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
            Debug.Log("Je suis le host et je ferme la session");
            NetworkManager.Singleton.Shutdown();
            
        }

        Cursor.visible = true;


    }
}
