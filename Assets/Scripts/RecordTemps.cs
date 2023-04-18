using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
public class RecordTemps : MonoBehaviour
{
    public TMP_Text timeText;
    private const string Path = "Assets/Sauvegarde Joueur Solo/Sauvegarde.txt";
    void Update()
    {
        List<string> liste = File.ReadAllLines(Path).ToList();
        int numeroNiveau = GestionJeuSolo.niveauActuel;
        char[] ligne = liste[numeroNiveau - 1].ToCharArray();
        string ancienTemps = Sauvegarde.GetAncienTemps(ligne);
        timeText.text = ancienTemps;
    }
}