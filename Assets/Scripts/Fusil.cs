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
    private void Update()
    {
        if(compteur < nombreMaxMunition)
        {
            if (Input.GetKeyDown(shootKey))
            {
                ++compteur;
                Instantiate(objectToSpawn, exitTransform.position, transform.rotation);
            }
        }
    }
}
