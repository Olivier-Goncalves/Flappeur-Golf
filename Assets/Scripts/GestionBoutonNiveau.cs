using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Fait par: Olivier Gonçalves
public class GestionBoutonNiveau : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener((() => GenererScene(int.Parse(gameObject.name))));
    }

    private void GenererScene(int index)
    {
        SceneManager.LoadScene($"Trou{index.ToString()}");
    }
}
