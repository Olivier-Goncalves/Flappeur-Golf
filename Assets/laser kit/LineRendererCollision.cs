using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererCollision : MonoBehaviour
{
    public LineRenderer Line;
    private void Update()
    {
        GenerateMeshCollider();
    }

    public void GenerateMeshCollider()
    {

        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }


        Mesh mesh = new Mesh();
        Line.BakeMesh(mesh, true);
        int[] meshIndices = mesh.GetIndices(0);
        int[] newIndices = new int[meshIndices.Length * 2];

        int j = meshIndices.Length - 1;
        for (int i = 0; i < meshIndices.Length; i++)
        {
            newIndices[i] = meshIndices[i];
            newIndices[meshIndices.Length + i] = meshIndices[j];
        }
        mesh.SetIndices(newIndices, MeshTopology.Triangles, 0);
        
    }
}

