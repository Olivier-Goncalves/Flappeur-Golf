using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

// Fait par Guillaume Flamand
public class NiveauProcédural : MonoBehaviour
{
    [SerializeField] private Vector3 positionDepart;
    [SerializeField] private int longueurCorridor = 20;
    [SerializeField] private int nombreDeCorridor = 15;
    [SerializeField] private GameObject plancher;
    [SerializeField] private GameObject mur;
    [SerializeField] private GameObject lanceurOndes;
    [SerializeField] private GameObject lanceurBouleDeFeu;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject zoneInverseGravité;
    [SerializeField] private GameObject zoneAccelereGravité;
    [SerializeField] private GameObject zoneAccelereEtInverseGravité;
    [SerializeField] private GameObject drapeau;
    [SerializeField] private Canvas menuPause;
    [SerializeField] private Button boutonRetourPartie;
    [SerializeField] private Button boutonRetourMenu;
    List<Vector3> positionPotentiellesChambres = new();
    
    private List<List<Vector3>> chambres = new();
    private List<List<Vector3>> corridors = new();
    private List<Vector3> positionsPlanchers = new();
    private static Vector3 directionCorridorPrécédent;
    private bool pause;

    private GameObject parent;
    public Button boutonGenerer;
    public Button boutonRecommencer;
    public RawImage fond;
    public RawImage fondDébut;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPause.enabled = !menuPause.enabled;
        }
    }

    private void Awake()
    {
        GestionJeuSolo.niveauActuel = 10;
        fondDébut.enabled = true;
        boutonRetourMenu.onClick.AddListener(()=> 
        {
            SceneManager.LoadScene("Solo");
        
        });
        boutonRetourPartie.onClick.AddListener(() =>
        {
            menuPause.enabled = false;
        });
        boutonRecommencer.onClick.AddListener(() =>
        {
            Recommencer();
            boutonRecommencer.GetComponentInParent<Canvas>().enabled = false;
            boutonRecommencer.enabled = false;
            fond.enabled = false;
        });
        boutonRecommencer.enabled = false;
        boutonRecommencer.GetComponentInParent<Canvas>().enabled = false;
        
        boutonGenerer.onClick.AddListener(() =>
        {
            parent = new GameObject("Niveau Aléatoire");
            CréerNiveau();
            boutonGenerer.GetComponentInParent<Canvas>().enabled = false;
            boutonGenerer.enabled = false;
            fondDébut.enabled = false;
            fond.enabled = false;
        });
    }
    private void Recommencer()
    {
        Destroy(parent);
        positionsPlanchers = new List<Vector3>();
        positionPotentiellesChambres = new List<Vector3>();
        parent = new GameObject();
        parent.name = "Niveau Aléatoire";
        chambres = new List<List<Vector3>>();
        CréerNiveau();
    }
    
    private void CréerNiveau()
    {
        CréerCorridors();
        CreerChambres();
        
        int indiceChambrePlusLoin = TrouverIndiceChambreLaPlusLoin(transform.TransformPoint(chambres[0][0]));
        int indiceChambre0 = TrouverIndiceChambreLaPlusLoin(transform.TransformPoint(chambres[indiceChambrePlusLoin][0]));

        for(int i = 0; i < chambres.Count; ++i)
        {
            GameObject chambre = new GameObject("Chambre #" + i);
            List<int> positionsObstacles = PositionsObstacles();
            
            for (int j = 0; j < chambres[i].Count; ++j)
            {
                GameObject nouveauPlancher = Instantiate(plancher, chambres[i][j], transform.rotation); 
                nouveauPlancher.transform.SetParent(chambre.transform);
                GameObject nouveauPlafond = Instantiate(plancher, chambres[i][j] + new Vector3(0,mur.transform.localScale.y/2.0f,0), transform.rotation);
                nouveauPlafond.transform.SetParent(chambre.transform);
                
                if (i == indiceChambre0)
                {
                    if (j == 85)
                    {
                        Spawns.spawnActuel = chambres[i][j] + new Vector3(0,0.5f,0);
                    }
                }
                else if (i == indiceChambrePlusLoin)
                {
                    if (j == 60)
                    {
                        GameObject trou = Instantiate(drapeau, chambres[i][j] + new Vector3(0,0.25f,0), transform.rotation);
                        trou.name = "Trou";
                        trou.transform.SetParent(parent.transform);
                    }
                }
                else
                {
                    if (positionsObstacles.Contains(j))
                    {
                        GameObject bdf = Instantiate(lanceurBouleDeFeu, chambres[i][j] + new Vector3(0,0.5f,0), transform.rotation);
                        bdf.transform.rotation = Quaternion.Euler(0,UnityEngine.Random.Range(0,2) == 0 ? -90 : 180,0);
                        bdf.transform.SetParent(chambre.transform);
                        GameObject nouveauLaser = Instantiate(laser, chambres[i][j] + new Vector3(0, 0.5f, 0), transform.rotation);
                        nouveauLaser.transform.rotation = Quaternion.Euler(0,UnityEngine.Random.Range(0,2) == 0 ? -90 : 45,0);
                        nouveauLaser.transform.SetParent(chambre.transform);

                        GameObject zone;
                        switch (UnityEngine.Random.Range(0, 3))
                        {
                            case 0:
                                zone = Instantiate(zoneInverseGravité,chambres[i][j], transform.rotation);
                                break;
                            case 1:
                                zone = Instantiate(zoneAccelereGravité,chambres[i][j], transform.rotation);
                                break;
                            case 2:
                                zone = Instantiate(zoneAccelereEtInverseGravité,chambres[i][j], transform.rotation);
                                break;
                            default:
                                zone = new GameObject();
                                break;
                        }
                        zone.transform.SetParent(chambre.transform);
                    }
                    if (j == UnityEngine.Random.Range(0,110))
                    {
                        GameObject onde = Instantiate(lanceurOndes, chambres[i][j] + new Vector3(0,4f,0), transform.rotation);
                        onde.transform.SetParent(chambre.transform);
                    }
                }
            }
            chambre.transform.SetParent(parent.transform);
        }
        InstancierCorridors();
        InstancierMurs();

        GameObject.Find("JoueurLocal").transform.position = Spawns.spawnActuel;
    }

    private List<int> PositionsObstacles()
    {
        List<int> positions = new List<int>();
        while (positions.Count < 5)
        {
            int nouvellePosition = UnityEngine.Random.Range(0, 109);
            if (!positions.Contains(nouvellePosition))
            {
                positions.Add(nouvellePosition);
            }
        }
        return positions;
    }
    private void InstancierMurs()
    {
        GameObject murs = new GameObject("Murs");
        foreach (var position in TrouverMursDansDirection(positionsPlanchers))
        {
            GameObject nouveauMur = Instantiate(mur, transform.TransformPoint(position), transform.rotation);
            nouveauMur.transform.SetParent(murs.transform);
        }
        murs.transform.SetParent(parent.transform);
    }

    private void InstancierCorridors()
    {
        for (int i = 0; i < corridors.Count; ++i)
        {
            GameObject corridor = new GameObject("Corridor #" + i);

            for (int j = 0; j < corridors[i].Count; ++j)
            {
                GameObject nouveauPlancher = Instantiate(plancher, transform.TransformPoint(corridors[i][j]), transform.rotation); 
                nouveauPlancher.transform.SetParent(corridor.transform);
                GameObject nouveauPlafond = Instantiate(plancher, transform.TransformPoint(corridors[i][j]) + new Vector3(0,mur.transform.localScale.y/2.0f,0), transform.rotation);
                nouveauPlafond.transform.SetParent(corridor.transform);
            }
            corridor.transform.SetParent(parent.transform);
        }
    }
    private int TrouverIndiceChambreLaPlusLoin(Vector3 repere)
    {
        // Pour le calcul de la distance, je prend toujouors le premier bloc de la pièce. En effet, la piece est constitué de 120 blocs, pour le plancher, donc pour que ca soit plus simple, je calcule toujuors avec le bloc à l'indice 0.
        
        float distance = 0;
        int indice = 0;
        
        for (int i = 1; i < chambres.Count; ++i)
        {
            float distanceActuelle = Vector3.Distance(repere, transform.TransformPoint(chambres[i][0]));
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
        
        for (int i = 0; i < nombreDeCorridor; ++i)
        {
            List<Vector3> corridors = CorridorsAléatoires(positionActuelle, longueurCorridor);
            
            positionActuelle = corridors[corridors.Count - 1];
            
            positionPotentiellesChambres.Add(positionActuelle);
            
            this.corridors.Add(corridors);
            foreach (var position in corridors)
            {
                positionsPlanchers.Add(position);
            }
        }
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
    
    private void CreerChambres()
    {
        int nbDeCHambresACreer = Mathf.RoundToInt(positionPotentiellesChambres.Count);

        List<Vector3> chambresACreer = positionPotentiellesChambres.OrderBy(x => Guid.NewGuid()).Take(nbDeCHambresACreer).ToList();

        for (int i = 0; i < chambresACreer.Count; ++i)
        {
            CreerChambre(chambresACreer[i]);
        }
    }
    private void CreerChambre(Vector3 position) => CreerPlancherChambre(position);
    
    private void CreerPlancherChambre(Vector3 positionDepart)
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
            estEgal = DeuxListesÉgales(chambre, pointsMonde, estEgal);
        }
        if (!estEgal)
        {
            chambres.Add(pointsMonde);
            foreach (var position in points)
            {
                positionsPlanchers.Add(position);
            }
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
        if (directionCorridorPrécédent != Vector3.zero)
        {
            while (DirectionContraire(direction))
            {
                Debug.Log("Contraire");
                direction = DirectionAleatoire();
            }
        }
        Vector3 positionActuelle = positionDepart;
        
        corridor.Add(positionActuelle);
        for (int i = 0; i < longueurCorridor; ++i)
        {
            positionActuelle += direction;
            corridor.Add(positionActuelle);
        }

        directionCorridorPrécédent = direction;
        return corridor;
    }
    private static bool DirectionContraire(Vector3 direction)
    {
        if (direction == _directionsCardinales[2] && directionCorridorPrécédent == _directionsCardinales[0])
        {
            // Direction précédente est devant et direction actuelle est derrière
            return true;
        }
        if (direction == _directionsCardinales[0] && directionCorridorPrécédent == _directionsCardinales[2])
        {
            // Direction précédente est derrière et direction actuelle est devant 
            return true;
        }
        if (direction == _directionsCardinales[3] && directionCorridorPrécédent == _directionsCardinales[1])
        {
            // Direction précédente est droite et direction actuelle est gauche
            return true;
        }
        if (direction == _directionsCardinales[1] && directionCorridorPrécédent == _directionsCardinales[3])
        {
            // Direction précédente est gauche et direction actuelle est droite
            return true;
        }
        return false;
    }
    private static List<Vector3> CréerCheminPlancherChambre()
    {
        List<Vector3> chemin = new List<Vector3>();
        int x = 25;

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
}

