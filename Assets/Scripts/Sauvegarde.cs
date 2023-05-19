using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Sauvegarde : MonoBehaviour
{
    private const string Path = "Assets/Resources/Sauvegarde.txt";
    public static void CréerSauvegarde(string temps)
    {
        List<string> liste = File.ReadAllLines(Path).ToList();
        
        int indiceNiveau = GestionJeuSolo.niveauActuel - 1;
        
        char[] ligne = liste[indiceNiveau].ToCharArray();

        int ancienNbFlaps = int.Parse(GetAncienNombreFlap(ligne));
        string ancienTemps = GetAncienTemps(ligne);

        liste[indiceNiveau] = GestionJeuSolo.niveauActuel + " ";

        int nbSautActuel = GameObject.Find("JoueurLocal").GetComponent<Saut>().nbSauts;
        
        if (nbSautActuel < ancienNbFlaps && TempsMeilleur(ancienTemps, temps))
        {
            liste[indiceNiveau] += nbSautActuel + " " + temps;
        }
        else if (nbSautActuel < ancienNbFlaps)
        {
            liste[indiceNiveau] += nbSautActuel + " " + ancienTemps;
        }
        else if (TempsMeilleur(ancienTemps, temps))
        {
            liste[indiceNiveau] += ancienNbFlaps + " " + temps;
        }
        else
        {
            liste[indiceNiveau] += ancienNbFlaps + " " + ancienTemps;
        }
        File.WriteAllLines(Path, liste.ToArray());
        AssetDatabase.ImportAsset(Path); 
    }

    public static string GetAncienNombreFlap(char[] ligne)
    {
        string nbFlap = "";
        int compteur = GestionJeuSolo.niveauActuel == 10 ? 3 : 2;
        while (ligne[compteur] != ' ')
        {
            nbFlap += ligne[compteur];
            compteur++;
        }
        return nbFlap;
    }

    public static string GetAncienTemps(char[] ligne)
    {
        string temps = String.Empty;
        for (int i = ligne.Length - 8; i < ligne.Length; ++i)
        {
            temps += ligne[i];
        }
        return temps;
    }
    private static bool TempsMeilleur(string ancienTemps, string nouveauTemps) => FormatterTemps(nouveauTemps) < FormatterTemps(ancienTemps);
    private static int FormatterTemps(string temps)
    {
        int premierChiffre = int.Parse(temps[0].ToString() + temps[1].ToString());
        int deuxiemeChiffre = int.Parse(temps[3].ToString() + temps[4].ToString());
        int troisiemeChiffre = int.Parse(temps[6].ToString() + temps[7].ToString());

        // Temps en millisecondes
        return premierChiffre * 60000 + deuxiemeChiffre * 1000 + troisiemeChiffre;
    }
}
