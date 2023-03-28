using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class Laser : MonoBehaviour
{
    private int bondMax = 20;
    private int compteur ;
    private LineRenderer laser;
    private Vector3 Vecteur;
    private void Start()
    {
        laser = GetComponent<LineRenderer>();
        laser.positionCount = 20;
    }
    private void Update()
    {
        compteur = 0;
        castLaser(transform.position + Vecteur, transform.up);
    }
    public void castLaser(Vector3 position , Vector3 direction)
    {
        laser.SetPosition(0, transform.position + Vecteur );

        int layerLaser = 1 << 11;
        
        
        
        
        for (int i=0; i < bondMax; i++ )
        {
            Ray rayon = new Ray(position, direction);
            
            RaycastHit touche;
            if(compteur < bondMax - 1)
                compteur++;
            if(Physics.Raycast(rayon , out touche , 300))
            {
                position = touche.point;
                direction = Vector3.Reflect(direction, touche.normal);
                laser.SetPosition(compteur, touche.point);
                if (touche.transform.tag == "Player")
                {
                    touche.transform.GetComponent<Collision>().CollisionLaser();
                }
                if (touche.transform.tag != "Mirror")
                {
                    for (int j = (i + 1); j < bondMax; j++)
                    {
                        laser.SetPosition(j, touche.point);
                    }
                    break;
                }
                else 
                {
                    laser.SetPosition(compteur, touche.point);
                }
            }
            else
            {
                laser.SetPosition(compteur, rayon.GetPoint(300));
            }
        }
    }
}
