using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerationNiveauAleatoire : MonoBehaviour
{
    private GameObject cube;

    private GameObject plancher;
    private void Awake()
    {
        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        CreerPlancher();
        AjouterObstacles();
    }
    private void CreerPlancher()
    {
        plancher = GameObject.CreatePrimitive(PrimitiveType.Plane);

        plancher.transform.localScale = new Vector3(Random.Range(1,10), 2, Random.Range(1,10));
        
    }
    private void AjouterObstacles()
    {
        int cpt = 0;
        Vector3[] vertices = { plancher.GetComponent<MeshFilter>().sharedMesh.vertices[0],
                                plancher.GetComponent<MeshFilter>().sharedMesh.vertices[10],
                                plancher.GetComponent<MeshFilter>().sharedMesh.vertices[110],
                                plancher.GetComponent<MeshFilter>().sharedMesh.vertices[120]};
        foreach (Vector3 vertice in vertices)
        {
            Debug.Log(cpt+ ": "+ vertice);
            cpt++;
        }
        GameObject temp;
        for(int i = 0; i < Random.Range(10, 20); i++)
        {
            temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            temp.transform.localScale = new Vector3(4, 5, 2);
            temp.transform.position = new Vector3(Random.Range(vertices[0].x, vertices[1].x), 1, Random.Range(vertices[0].z, vertices[3].z));
        }
    }
    
    private float GenererPositionSelonPlancher()
    {
        float nombre = 0;

        Vector3[] vertices = { plancher.GetComponent<MeshFilter>().sharedMesh.vertices[0],
                                plancher.GetComponent<MeshFilter>().sharedMesh.vertices[10],
                                plancher.GetComponent<MeshFilter>().sharedMesh.vertices[110],
                                plancher.GetComponent<MeshFilter>().sharedMesh.vertices[120]};
        
        return nombre;
    }
}
