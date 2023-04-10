using System;
using System.Collections.Generic;
using System.Linq;
using Shapes2D;
using UnityEngine;
public class Corridors : MonoBehaviour
{
    [SerializeField] private Vector3 positionDepart;
    [SerializeField] private int longueurCorridor = 20;
    [SerializeField] private int nombreDeCorridor = 15;
    [SerializeField] private GameObject plancher;
    [SerializeField] private GameObject mur;
    [SerializeField] private GameObject joueur;
    [SerializeField] private GameObject lanceurOndes;
    [SerializeField] private GameObject lanceurBouleDeFeu;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject zoneInverseGravité;
    [SerializeField] private GameObject zoneAccelereGRavité;
    [SerializeField] private GameObject zoneAccelereEtInverseGravité;
    [SerializeField] private GameObject drapeau;
    [SerializeField] private Material premiereChambre;
    [SerializeField] private Material derniereChambre;
    
    List<Vector3> positionsPlanchers = new();
    List<Vector3> positionPotentiellesChambres = new();
    
    private List<Chambre> chambres = new();

    private GameObject parent;

    private GameObject spawn;

    private void Awake()
    {
        parent = new GameObject();
        parent.name = "Niveau Aléatoire";
        spawn = new GameObject();
        CréerNiveau();
        Instantiate(joueur, spawn.transform.position, transform.rotation);
    }
    
    public void Recommencer()
    {
        Destroy(parent);
        positionsPlanchers = new List<Vector3>();
        positionPotentiellesChambres = new List<Vector3>();
        chambres = new List<Chambre>();
        parent = new GameObject();
        parent.name = "Niveau Aléatoire";
        spawn = new GameObject();
        CréerNiveau();
    }
    private void CréerNiveau()
    {
        CréerCorridors();
        
        List<Vector3> positionChambres = CreerChambres();
        
        positionsPlanchers = positionsPlanchers.Union(positionChambres).Distinct().ToList();

        int indiceChambrePlusLoin = TrouverIndiceChambreLaPlusLoin(transform.TransformPoint(chambres[0].positions[0]));
        int indiceChambre0 = TrouverIndiceChambreLaPlusLoin(transform.TransformPoint(chambres[indiceChambrePlusLoin].positions[0]));

        for(int i = 0; i < chambres.Count; ++i)
        {
            GameObject chambre = new GameObject();
            chambre.name = "Chambre #" + i;

            for (int j = 0; j < chambres[i].positions.Count; ++j)
            {
                GameObject nouveauPlancher = Instantiate(plancher, chambres[i].positions[j], transform.rotation); 
                nouveauPlancher.transform.SetParent(chambre.transform);
                GameObject nouveauPlafond = Instantiate(plancher, chambres[i].positions[j] + new Vector3(0,mur.transform.localScale.y/2.0f,0), transform.rotation);
                nouveauPlafond.transform.SetParent(chambre.transform);
                
                if (i == indiceChambre0)
                {
                    nouveauPlancher.GetComponent<Renderer>().material = premiereChambre; 
                    
                    if (j == 85)
                    {
                        spawn.transform.position = chambres[i].positions[j] + new Vector3(0,0.5f,0);
                        spawn.name = "Spawn";
                    }
                }
                else if (i == indiceChambrePlusLoin)
                {
                    nouveauPlancher.GetComponent<Renderer>().material = derniereChambre; 

                    if (j == 60)
                    {
                        GameObject trou = Instantiate(drapeau, chambres[i].positions[j], transform.rotation);
                        trou.transform.SetParent(parent.transform);
                    }
                }
                else
                {
                    if (j == 20 || j == 45 || j == 100 || j == 75)
                    {
                        GameObject bdf = Instantiate(lanceurBouleDeFeu, chambres[i].positions[j] + new Vector3(0,0.5f,0), transform.rotation);
                        bdf.transform.rotation = Quaternion.Euler(0,UnityEngine.Random.Range(0,2) == 0 ? -90 : 90,0);
                        bdf.transform.SetParent(chambre.transform);
                        GameObject nouveauLaser = Instantiate(laser, chambres[i].positions[j] + new Vector3(0, 0.5f, 0), transform.rotation);
                        nouveauLaser.transform.rotation = Quaternion.Euler(0,UnityEngine.Random.Range(0,2) == 0 ? -90 : 90,0);
                        nouveauLaser.transform.SetParent(chambre.transform);
                    }
                }
            }
            chambre.transform.SetParent(parent.transform);
        }
        
        List<Vector3> positionsMurs = TrouverMursDansDirection(positionsPlanchers);
        GameObject murs = new GameObject();
        murs.name = "Murs";
        foreach (var position in positionsMurs)
        {
            GameObject nouveauMur = Instantiate(mur, transform.TransformPoint(position), transform.rotation);
            nouveauMur.transform.SetParent(murs.transform);
        }
        murs.transform.SetParent(parent.transform);
    }
    private int TrouverIndiceChambreLaPlusLoin(Vector3 repere)
    {
        // Pour le calcul de la distance, je prend toujouors le premier bloc de la pièce. En effet, la piece est constitué de 120 blocs, pour le plancher, donc pour que ca soit plus simple, je calcule toujuors avec le bloc à l'indice 0.
        
        float distance = 0;
        int indice = 0;
        
        for (int i = 1; i < chambres.Count; ++i)
        {
            float distanceActuelle = Vector3.Distance (repere, transform.TransformPoint(chambres[i].positions[0]));
            if (distanceActuelle > distance)
            {
                distance = distanceActuelle;
                indice = i;
            }
        }
        return indice;
    }
    private void CréerCorridors()
    {
        Vector3 positionActuelle = positionDepart;
        positionPotentiellesChambres.Add(positionActuelle);

        GameObject planchersCorridors = new GameObject();
        planchersCorridors.name = "Planchers corridors";
        
        for (int i = 0; i < nombreDeCorridor; ++i)
        {
            List<Vector3> corridors = CorridorsAléatoires(positionActuelle, longueurCorridor);
            positionActuelle = corridors[corridors.Count - 1];
            positionPotentiellesChambres.Add(positionActuelle);
            positionsPlanchers = positionsPlanchers.Union(corridors).Distinct().ToList();
            
            foreach (var position in positionsPlanchers)
            {
                GameObject nouveauPlancher = Instantiate(plancher, transform.TransformPoint(position), transform.rotation);
                nouveauPlancher.transform.SetParent(planchersCorridors.transform);
                GameObject nouveauPlafond = Instantiate(plancher, transform.TransformPoint(position) + new Vector3(0,35.0f/2f,0), transform.rotation);
                nouveauPlafond.transform.SetParent(planchersCorridors.transform);
            }
        }
        planchersCorridors.transform.SetParent(parent.transform.transform);
    }
    
    private static List<Vector3> TrouverMursDansDirection(List<Vector3> positionsPlanchers)
    {
        List<Vector3> positionMurs = new List<Vector3>();
        foreach (var position in positionsPlanchers)
        {
            foreach (var direction in _directionsCardinales)
            {
                Vector3 positionVoisin = position + direction;
                if (!positionsPlanchers.Contains(positionVoisin))
                {
                    positionMurs.Add(positionVoisin);
                }
            }
        }
        return positionMurs;
    }
    
    private List<Vector3> CreerChambres()
    {
        List<Vector3> positionsChambres = new List<Vector3>();
        int nbDeCHambresACreer = Mathf.RoundToInt(positionPotentiellesChambres.Count);

        List<Vector3> chambresACreer = positionPotentiellesChambres.OrderBy(x => Guid.NewGuid()).Take(nbDeCHambresACreer).ToList();

        for (int i = 0; i < chambresACreer.Count; ++i)
        {
            List<Vector3> plancherChambre = CreerChambre(chambresACreer[i]);
            positionsChambres = positionsChambres.Union(plancherChambre).Distinct().ToList();
        }
        return positionsChambres;
    }
    private List<Vector3> CreerChambre(Vector3 position) => positionsPlanchers = positionsPlanchers.Union(CreerPlancherChambre(position)).Distinct().ToList();
    
    private List<Vector3> CreerPlancherChambre(Vector3 positionDepart)
    {
        List<Vector3> chemin = new();

        chemin.Add(positionDepart);
        
        Vector3 positionPrecedente = positionDepart;

        List<Vector3> cheminPourCreerChambre = CréerCheminPlancherChambre();
        
        for (int i = 0; i < cheminPourCreerChambre.Count; ++i)
        {
            Vector3 nouvellePosition = positionPrecedente + cheminPourCreerChambre[i];
            chemin.Add(nouvellePosition);
            positionPrecedente = nouvellePosition;
        }
        AjouterPointsDuMonde(chemin);
        
        return chemin;
    }

    private void AjouterPointsDuMonde(List<Vector3> points)
    {
        List<Vector3> pointsMonde = new List<Vector3>();
        foreach (var point in points)
        {
            pointsMonde.Add(transform.TransformPoint(point));
        }

        bool estEgal = false;
        foreach (var chambre in chambres)
        {
            estEgal = DeuxListesÉgales(chambre.positions, pointsMonde, estEgal);
        }
        if (!estEgal)
        {
            chambres.Add(new Chambre(pointsMonde));
        }
    }

    private bool DeuxListesÉgales(List<Vector3> liste1, List<Vector3> liste2, bool egales)
    {
        for (int i = 0; i < liste1.Count; ++i)
        {
            if (liste1[i] == liste2[i])
            {
                egales = true;
            }
        }
        return egales;
    }
    private static List<Vector3> CorridorsAléatoires(Vector3 positionDepart, int longueurCorridor)
    {
        List<Vector3> corridor = new();
        Vector3 direction = DirectionAleatoire();
        Vector3 positionActuelle = positionDepart;
        
        corridor.Add(positionActuelle);
        for (int i = 0; i < longueurCorridor; ++i)
        {
            positionActuelle += direction;
            corridor.Add(positionActuelle);
        }
        return corridor;
    }

    private static List<Vector3> CréerCheminPlancherChambre()
    {
        List<Vector3> chemin = new List<Vector3>();
        int x = 25;
        int z = -25;

        while (x > -25)
        {
            for (int i = 0; i < 10; ++i)
            {
                chemin.Add(new Vector3(0,0,5));
            }
            if (x != -20)
            {
                chemin.Add(new Vector3(-5,0,-50));
            }
            x -= 5;
        }
        return chemin;
    }
    private static List<Vector3Int> _directionsCardinales = new List<Vector3Int>()
    {
        new (0, 0, 5), // Devant
        new (5, 0, 0), // Droite
        new (0, 0, -5), // Derrière
        new (-5, 0, 0) // Gauche
    };
    private static Vector3Int DirectionAleatoire() => _directionsCardinales[UnityEngine.Random.Range(0, _directionsCardinales.Count)];
    private class Chambre
    {
        public readonly List<Vector3> positions;
        public Chambre(List<Vector3> positions)
        {
            this.positions = positions;
        }
    }
}

