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
            {
                GestionJeuSolo.niveauActuel = int.Parse(gameObject.name);
                Spawns.spawnActuel = Spawns.spawns[GestionJeuSolo.niveauActuel - 1];
                GenererScene();
                Debug.Log(Spawns.spawns[GestionJeuSolo.niveauActuel - 1]);
                Debug.Log("Spawn actuel = " +  Spawns.spawnActuel);
            } 
        });
    }

    private void GenererScene()
    {
        GameObject.Find("GameManagerSolo").GetComponent<GestionJeuSolo>().Ressusciter();
        canvas.enabled = false;
    }
}
