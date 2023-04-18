using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class Sauvegarde : MonoBehaviour
{
    private const string Path = "Assets/Sauvegarde Joueur Solo/Sauvegarde.txt";
    private static int[] indices = new[] { 5, 6, 8, 9, 11, 12 };
    
    public static void CréerSauvegarde(string temps)
    {
        List<string> liste = File.ReadAllLines(Path).ToList();
        int numeroNiveau = GestionJeuSolo.niveauActuel;
        
        char[] ligne = liste[numeroNiveau - 1].ToCharArray();

        string ancienNbFlaps = GetAncienNombreFlap(ligne);
        string ancienTemps = GetAncienTemps(ligne);
        
        
        
        Debug.Log(ancienTemps);
        Debug.Log(ancienNbFlaps);
        
        if (Jump.nbSauts < int.Parse(ancienNbFlaps) && ComparerTemps(ancienTemps, temps))
        {
            Debug.Log("Meilleur temps");
            liste[GestionJeuSolo.niveauActuel - 1] = $"{GestionJeuSolo.niveauActuel} {Jump.nbSauts} {temps}";
        }
        else if (Jump.nbSauts < int.Parse(ancienNbFlaps))
        {
            liste[GestionJeuSolo.niveauActuel - 1] = $"{GestionJeuSolo.niveauActuel} {Jump.nbSauts} {ancienTemps}";
        }
        else if (ComparerTemps(ancienTemps, temps))
        {
            liste[GestionJeuSolo.niveauActuel - 1] = $"{GestionJeuSolo.niveauActuel} {ancienNbFlaps} {temps}";
        }
        
        File.WriteAllLines(Path, liste.ToArray());
    }

    private static string GetAncienNombreFlap(char[] ligne)
    {
        string nbFlap = "";
        int compteur = 2;
        while (ligne[compteur] != ' ')
        {
            //Debug.Log(ligne[compteur]);
            nbFlap += ligne[compteur];
            compteur++;
            
        }
        return nbFlap;
    }

    private static string GetAncienTemps(char[] ligne)
    {
        string temps = "";
        for (int i = indices[0]; i < ligne.Length; ++i)
        {
            temps += ligne[i];
        }
        return temps;
    }

    
    private static bool ComparerTemps(string ancienTemps, string nouveauTemps)
    {
        return FormatterTemps(nouveauTemps) < FormatterTemps(ancienTemps);
    }
    

    
    private static int FormatterTemps(string temps)
    {
        int premierChiffre = int.Parse(temps[0].ToString() + temps[1].ToString());
        int deuxiemeChiffre = int.Parse(temps[3].ToString() + temps[4].ToString());
        int troisiemeChiffre = int.Parse(temps[6].ToString() + temps[7].ToString());

        return premierChiffre * 60000 + deuxiemeChiffre * 1000 + troisiemeChiffre;
    }
    
}
