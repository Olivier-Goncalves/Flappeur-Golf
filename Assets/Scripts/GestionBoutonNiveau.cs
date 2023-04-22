using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Shapes2D;
using UnityEngine;
using UnityEngine.UI;

// Fait par: Olivier Gonçalves
public class GestionBoutonNiveau : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private const string Path = "Assets/Sauvegarde Joueur Solo/Sauvegarde.txt";
    private const string or = "#F8FF00";
    private const string argent = "#AFB0B2";
    private const string bronze = "#964732";
    
    private void Awake()
    {
        List<string> liste = File.ReadAllLines(Path).ToList();
        int numeroNiveau = int.Parse(gameObject.name);;
        char[] ligne = liste[numeroNiveau - 1].ToCharArray();
        int ancienNombreFlap = int.Parse(Sauvegarde.GetAncienNombreFlap(ligne));
        List<GameObject> enfants = GetAllChilds(gameObject);

        foreach (var enfant in enfants)
        {
            if(enfant.name == "Médaille")
            {
                if (ancienNombreFlap != 0)
                {
                    if (ancienNombreFlap <= GestionJeuSolo.CoupsParTrou[numeroNiveau-1,0])
                    {
                        AssignerCouleur(or, enfant);
                    }
                    else if (ancienNombreFlap <= GestionJeuSolo.CoupsParTrou[numeroNiveau-1, 1])
                    {
                        AssignerCouleur(argent, enfant);
                    }
                    else if (ancienNombreFlap <= GestionJeuSolo.CoupsParTrou[numeroNiveau-1, 2])
                    {
                        AssignerCouleur(bronze, enfant);
                    }
                }
                else
                {
                    enfant.transform.GetComponent<Image>().color = Color.black;
                }
            }
        }
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!GameObject.Find("GameManagerSolo").GetComponent<GestionJeuSolo>().gameOn)
            {
                GestionJeuSolo.niveauActuel = int.Parse(gameObject.name);
                Spawns.spawnActuel = Spawns.spawns[GestionJeuSolo.niveauActuel - 1];
                GenererScene();
            } 
        });
    }

    private void AssignerCouleur(string couleur, GameObject gameObject)
    {
        Color nouvelleCouleur;
        if (ColorUtility.TryParseHtmlString(couleur, out nouvelleCouleur))
        {
            gameObject.transform.GetComponent<Image>().color = nouvelleCouleur;
        }
    }

    private void GenererScene()
    {
        GameObject.Find("GameManagerSolo").GetComponent<GestionJeuSolo>().Ressusciter();
        canvas.enabled = false;
    }
    public static List<GameObject> GetAllChilds(GameObject gameObject)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i< gameObject.transform.childCount; i++)
        {
            list.Add(gameObject.transform.GetChild(i).gameObject);
        }
        return list;
    }
    
}
