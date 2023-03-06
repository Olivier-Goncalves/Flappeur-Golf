using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEngine;
using Matrix = MathNet.Numerics.LinearAlgebra.Complex.Matrix;

public class CréerSpline : MonoBehaviour
{
    private Vector2[] pointsSpline; 
    private List<Vector3> points = new();

    private int current = 0;
    
    private bool inversé = false;
    private Vector<double> coefficients;
    
    private void Awake()
    {
        pointsSpline = new[] {new Vector2(0, 0), new(20, UnityEngine.Random.Range(21,25)), new(35, UnityEngine.Random.Range(13,20))};
        
        double premierX = pointsSpline[0].x;
        double deuxiemeX = pointsSpline[1].x;
        double troisiemeX = pointsSpline[2].x;

        double premierY = pointsSpline[0].y;
        double deuxiemeY = pointsSpline[1].y;
        double troisiemeY = pointsSpline[2].y;
        
        Vector<double> b =  Vector<double>.Build.Dense(new[] { premierY, deuxiemeY, deuxiemeY, troisiemeY, 0, 0, 0, 0});
        
        Matrix<double> A = Matrix<double>.Build.DenseOfArray(new[,] {
            
            { Math.Pow(premierX, 3), Math.Pow(premierX, 2), premierX, 1, 0, 0, 0, 0 },
            
            { Math.Pow(deuxiemeX, 3), Math.Pow(deuxiemeX, 2), deuxiemeX, 1, 0, 0, 0, 0 },
            
            { 0, 0, 0, 0, Math.Pow(deuxiemeX, 3), Math.Pow(deuxiemeX, 2), deuxiemeX, 1,},
            
            { 0, 0, 0, 0, Math.Pow(troisiemeX, 3), Math.Pow(troisiemeX, 2), troisiemeX, 1 },
            
            { Math.Pow(deuxiemeX,2)*3f, deuxiemeX*2f, 1, 0, -Math.Pow(deuxiemeX,2)*3f, -(deuxiemeX*2f), -1, 0},
            
            { deuxiemeX*6f, 2, 0, 0, -deuxiemeX*6f, -2, 0, 0},
            
            { premierX*6f, 2, 0, 0, 0, 0, 0, 0}, 
            
            { 0, 0, 0, 0, troisiemeX*6f, 2, 0, 0} 
        });
        coefficients = A.Solve(b);

        for (float i = 0; i <= pointsSpline[2].x; i += 0.5f)
        {
            points.Add(CréerPointSpline(i));
        }
    }
    private void Start()
    {
        transform.localPosition = points[current];
    }

    private Vector3 CréerPointSpline(float x)
    {
        int index = x < pointsSpline[1].x ? 0 : 4; 
        return new Vector3(0, (float)(coefficients[index] * Math.Pow(x, 3) + coefficients[index + 1] * Math.Pow(x, 2) + coefficients[index + 2] * x + coefficients[index + 3]), x);
    }

    private void Update()
    {
        if (current < points.Count)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[current], Time.deltaTime * 15);
            
            if (transform.localPosition == points[current])
            {
                current = inversé ? current - 1 : current + 1;
            }
        }
        if (current == 0 && inversé)
        {
            inversé = false;
        }
        if (current == points.Count - 1)
        {
            inversé = true;
        }
    }
}
