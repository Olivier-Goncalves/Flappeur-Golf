using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Fait par: Louis-F�lix Cl�ment
public class GestionMenuAccueil : MonoBehaviour
{
    [SerializeField] private Button boutonMultijoueur;
    [SerializeField] private Button boutonSolo;
    

    private void Awake()
    {
        boutonMultijoueur.onClick.AddListener(G�n�rerSc�neMultijoueur);
        boutonSolo.onClick.AddListener(G�n�rerSc�neSolo);
    }

    private void G�n�rerSc�neMultijoueur()
    {
        SceneManager.LoadScene("Multijoueur");
    }

    private void G�n�rerSc�neSolo()
    {
        SceneManager.LoadScene("Solo");
    }

}
