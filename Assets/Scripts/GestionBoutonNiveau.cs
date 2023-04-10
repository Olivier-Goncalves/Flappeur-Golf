using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Fait par: Olivier Gonçalves
public class GestionBoutonNiveau : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!GameObject.Find("GameManagerSolo").GetComponent<GestionJeuSolo>().gameOn)
                GenererScene(int.Parse(gameObject.name));
        });
    }

    private void GenererScene(int index)
    {
        GameObject.Find("GameManagerSolo").GetComponent<GestionJeuSolo>().Ressusciter(index);
        canvas.enabled = false;
    }
}
