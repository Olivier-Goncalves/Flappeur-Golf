using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusil: MonoBehaviour
{
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform exitTransform;
    private int compteur = 0;
    private int nombreMaxMunition = 3;
    private float time = 0;
    private GameObject balle;
    private void Update()
    {
        if(compteur < nombreMaxMunition)
        {
            if (Input.GetKeyDown(shootKey))
            {
                ++compteur;
                balle = Instantiate(objectToSpawn, exitTransform.position, transform.rotation);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
