using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerationNiveauAleatoire : MonoBehaviour
{
    //private GameObject cube;

    //private GameObject plancher;
    //private void Awake()
    //{
    //    //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    CreerPlancher();
    //    AjouterObstacles();
    //}
    //private void CreerPlancher()
    //{
    //    plancher = GameObject.CreatePrimitive(PrimitiveType.Plane);

    //    plancher.transform.localScale = new Vector3(Random.Range(1,10), 2, Random.Range(1,10));

    //}
    //private void AjouterObstacles()
    //{
    //    int cpt = 0;
    //    Vector3[] vertices = { plancher.GetComponent<MeshFilter>().sharedMesh.vertices[0],
    //                            plancher.GetComponent<MeshFilter>().sharedMesh.vertices[10],
    //                            plancher.GetComponent<MeshFilter>().sharedMesh.vertices[110],
    //                            plancher.GetComponent<MeshFilter>().sharedMesh.vertices[120]};
    //    foreach (Vector3 vertice in vertices)
    //    {
    //        Debug.Log(cpt+ ": "+ vertice);
    //        cpt++;
    //    }
    //    GameObject temp;
    //    for(int i = 0; i < Random.Range(10, 20); i++)
    //    {
    //        temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //        temp.transform.localScale = new Vector3(4, 5, 2);
    //        temp.transform.position = new Vector3(Random.Range(vertices[0].x, vertices[1].x), 1, Random.Range(vertices[0].z, vertices[3].z));
    //    }
    //}

    //private float GenererPositionSelonPlancher()
    //{
    //    float nombre = 0;

    //    Vector3[] vertices = { plancher.GetComponent<MeshFilter>().sharedMesh.vertices[0],
    //                            plancher.GetComponent<MeshFilter>().sharedMesh.vertices[10],
    //                            plancher.GetComponent<MeshFilter>().sharedMesh.vertices[110],
    //                            plancher.GetComponent<MeshFilter>().sharedMesh.vertices[120]};

    //    return nombre;
    //}
    [SerializeField] private GameObject joueur;
    [SerializeField] private Material materielGreen;
    [SerializeField] private GameObject drapeau;
    [SerializeField] private GameObject spawnerBouleDeFeu;
    [SerializeField] private GameObject spawnerOndes;
    [SerializeField] private Material acid;
    [SerializeField] private Material sticky;

    private const int AcidZoneLayer = 7;
    private void Awake()
    {
        Instantiate(joueur, new Vector3(0, 0, 5), transform.rotation);
        GameObject mur1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mur1.transform.localScale = new Vector3(300, 40, 1);
        mur1.transform.rotation = Quaternion.Euler(0, 90, 0);

        GameObject mur2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mur2.transform.localScale = new Vector3(300, 40, 1);
        mur2.transform.rotation = Quaternion.Euler(0, -90, 0);

        GameObject mur3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mur3.transform.localScale = new Vector3(60, 40, 1);

        GameObject mur4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mur4.transform.localScale = new Vector3(60, 40, 1);

        GameObject plancher = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plancher.transform.localScale = new Vector3(5, 1, 30);


        GameObject plafond = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plafond.transform.localScale = new Vector3(5, 1, 30);
        plafond.transform.rotation = Quaternion.Euler(0, 0, -180);

        plancher.transform.position = new Vector3(0, -5, 0);
        plafond.transform.position = new Vector3(-7, 15, 0);
        mur1.transform.position = new Vector3(-15, -5, 15);
        mur2.transform.position = new Vector3(15, -5, 15);
        mur3.transform.position = new Vector3(0, -5, 150);
        mur4.transform.position = new Vector3(0, -5, 0);


        GameObject green = GameObject.CreatePrimitive(PrimitiveType.Cube);
        green.GetComponent<Renderer>().material = materielGreen;
        green.transform.position =
            new Vector3(UnityEngine.Random.Range(-15f, 15f), -5f, UnityEngine.Random.Range(100f, 150f));

        green.transform.localScale = new Vector3(10, 1f, 10);

        Instantiate(drapeau, green.transform.position + new Vector3(0, 0.5f, 0), transform.rotation);

        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                mur1.GetComponent<Renderer>().material = acid;
                mur1.layer = AcidZoneLayer;
                break;
            case 1:
                mur2.GetComponent<Renderer>().material = acid;
                mur2.layer = AcidZoneLayer;
                break;
            case 2:
                plafond.GetComponent<Renderer>().material = acid;
                plafond.layer = AcidZoneLayer;
                break;
            case 3:
                plancher.GetComponent<Renderer>().material = acid;
                plancher.layer = AcidZoneLayer;
                break;
        }


        for (int i = 0; i < 15; ++i)
        {
            Instantiate(spawnerBouleDeFeu,
                new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(-5, 5),
                    UnityEngine.Random.Range(0f, 150f)), Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 180), 0)));
        }
    }
}
