using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Fait par: Louis-Félix Clément | Remise 1
public class GestionMenuAccueil : MonoBehaviour
{
    [SerializeField] private Button boutonMultijoueur;
    [SerializeField] private Button boutonSolo;


    private void Awake()
    {
        boutonMultijoueur.onClick.AddListener(GenererSceneMultijoueur);
        boutonSolo.onClick.AddListener(GenererSceneSolo);
    }

    private void GenererSceneMultijoueur()
    {
        SceneManager.LoadScene("Multijoueur");
    }

    private void GenererSceneSolo()
    {
        SceneManager.LoadScene("Solo");
    }

}
