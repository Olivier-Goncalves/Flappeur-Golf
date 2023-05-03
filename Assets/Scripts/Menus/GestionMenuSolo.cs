using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Fait par: Olivier Gonçalves | Remise 1
public class GestionMenuSolo : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button btnRetour;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        GameObject.Find("Retour").GetComponent<Button>().onClick.AddListener(() => { canvas.enabled = true; });
        btnRetour.onClick.AddListener(()=> { SceneManager.LoadScene("Solo"); });
    }

    

}
