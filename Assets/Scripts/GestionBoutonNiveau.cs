using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Fait par: Olivier Goncalves
public class GestionBoutonNiveau : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private const string or = "#F8FF00";
    private const string argent = "#AFB0B2";
    private const string bronze = "#964732";
    private string[] couleursMedailles = { "#F8FF00", "#AFB0B2", "#964732" };


    private void Awake()
    {
        GestionJeuSolo.niveauActuel = 1;
        // Louis-Félix C a aidé pour cette partie puisque c'est lui qui a fait la sauvegarde
        var fichierTexte = Resources.Load<TextAsset>("Sauvegarde");
        List<string> listeMots = new List<string>(fichierTexte.text.Split('\n'));
        int numeroNiveau = int.Parse(gameObject.name);
        char[] ligne = listeMots[numeroNiveau - 1].ToCharArray();
        string line = "";
        for (int i = 0; i < ligne.Length; ++i)
        {
            line += ligne[i];
        }
        int ancienNombreFlap = int.Parse(Sauvegarde.GetAncienNombreFlap(ligne));
        List<GameObject> enfants = TrouverTousLesEnfants(gameObject);
        Image medaille = null;
        foreach (var enfant in enfants)
        {
            if(enfant.name == "Médaille")
            {
                medaille = enfant.GetComponent<Image>();
                break;
            }
        }

        if (ancienNombreFlap != 0)
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    if (ancienNombreFlap <= GestionJeuSolo.CoupsParTrou[numeroNiveau - 1, i])
                    {
                        AssignerCouleur(couleursMedailles[i], medaille.gameObject);
                        break;
                    }
                }
            }
            catch (NullReferenceException medailleException)
            {
                Debug.Log("Médaille n'existe pas");
            }
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!GameObject.Find("GameManagerSolo").GetComponent<GestionJeuSolo>().jeuEnCours)
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
    private static List<GameObject> TrouverTousLesEnfants(GameObject gameObject)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i< gameObject.transform.childCount; i++)
        {
            list.Add(gameObject.transform.GetChild(i).gameObject);
        }
        return list;
    }
}
