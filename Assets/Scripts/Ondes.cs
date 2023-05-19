using System.Collections.Generic;
using UnityEngine;

public class Ondes : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float fréquenceAngulaire = 10;
    [SerializeField] private float k = 25;
    [SerializeField] private float longueurOnde = 10f;
    [SerializeField] private Color color;

    private static int layerOnde = 14;
    private List<GameObject> ondes = new();
    
    private float time;
    
    private Quaternion[] angles = 
    {
        Quaternion.Euler(0,0,0),
        Quaternion.Euler(0,90,0),
        Quaternion.Euler(0,-90,0),
        Quaternion.Euler(0,180,0),
        Quaternion.Euler(-90,0,0),
        Quaternion.Euler(90,0,0), 
    };
    private void Awake()
    {
        for (int i = 0; i < angles.Length; ++i)
        {
            GameObject Onde = new GameObject("Onde stationnaire");
            
            Onde.transform.position = transform.position;

            List<Vector3> points = new List<Vector3>();

            for (float x = Onde.transform.position.z; x < Onde.transform.position.z + longueurOnde; x += longueurOnde / 20)
            {
                points.Add(new Vector3(0,CalculerY(x, 0),x));
            }
            for (int j = 0; j < points.Count; ++j)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = points[j];
                sphere.GetComponent<Renderer>().material.color = color;
                sphere.transform.localScale = new Vector3(1f, 1f, 1f);
                sphere.transform.SetParent(Onde.transform);
                sphere.AddComponent<SphereCollider>();
                sphere.layer = layerOnde;
            }
            Onde.transform.SetParent(transform);
            Onde.transform.rotation = angles[i];
            ondes.Add(Onde);
        }
    }
    private void Update()
    {
        for (int i = 0; i < ondes.Count; ++i)
        {
            foreach (Transform child in ondes[i].transform)
            {
                time += Time.deltaTime;
                child.transform.localPosition = new Vector3(0, CalculerY(child.transform.localPosition.z, time), child.transform.localPosition.z);
            }
        }
    }
    private float CalculerY(float x, float t) => amplitude * Mathf.Cos(fréquenceAngulaire * t) * Mathf.Sin(k * x);
}