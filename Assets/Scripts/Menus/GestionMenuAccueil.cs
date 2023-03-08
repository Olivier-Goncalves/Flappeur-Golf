using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Fait par: Louis-Félix Clément
public class GestionMenuAccueil : MonoBehaviour
{
    [SerializeField] private Button boutonMultijoueur;
    [SerializeField] private Button boutonSolo;
    

    private void Awake()
    {
        boutonMultijoueur.onClick.AddListener(GénérerScèneMultijoueur);
        boutonSolo.onClick.AddListener(GénérerScèneSolo);
    }

    private void GénérerScèneMultijoueur()
    {
        SceneManager.LoadScene("Multijoueur");
    }

    private void GénérerScèneSolo()
    {
        SceneManager.LoadScene("Solo");
    }

}
