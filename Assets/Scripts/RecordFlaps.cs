using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
public class RecordFlaps : MonoBehaviour
{
    public TMP_Text timeText;
    private const string Path = "Assets/Sauvegarde Joueur Solo/Sauvegarde.txt";
    void Update()
    {
        List<string> liste = File.ReadAllLines(Path).ToList();
        int numeroNiveau = GestionJeuSolo.niveauActuel;
        char[] ligne = liste[numeroNiveau - 1].ToCharArray();
        string ancienNombreFlap = Sauvegarde.GetAncienNombreFlap(ligne);
        timeText.text = ancienNombreFlap;
    }
}