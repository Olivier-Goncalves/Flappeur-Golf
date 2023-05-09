using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Fait par: Louis-Félix Clément | Remise 2
public class GestionMenuChoixSolo : MonoBehaviour
{
    [SerializeField] private Button btnAleatoire;
    [SerializeField] private Button btnNiveaux;
    [SerializeField] private Button btnRetour;
    private void Awake()
    {
        btnAleatoire.onClick.AddListener(() => { SceneManager.LoadScene("AleatoireSolo"); });
        btnNiveaux.onClick.AddListener(() => { SceneManager.LoadScene("TrouSolo"); });
        btnRetour.onClick.AddListener(()=> { SceneManager.LoadScene("MenuAccueil"); });
    }
}
