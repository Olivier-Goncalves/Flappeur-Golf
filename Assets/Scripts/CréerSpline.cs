using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEngine;
using Matrix = MathNet.Numerics.LinearAlgebra.Complex.Matrix;
// Fait par: Guillaume Flamand
public class CréerSpline : MonoBehaviour
{
    [SerializeField] private float vitesse = 100;
    private Time horloge;
    private Vector2[] pointsSpline; 
    private List<Vector3> points = new();

    private int current = 0;
    
    private bool inversé = false;
    private Vector<double> coefficients;

    private float elapsedTime;
    private void Awake()
    {
        pointsSpline = new[] {new Vector2(0, 0), new(20, UnityEngine.Random.Range(21,25)), new(35, UnityEngine.Random.Range(13,20))};

        Vector<double> b = CréerMatriceB(pointsSpline[0].y, pointsSpline[1].y, pointsSpline[2].y);
        Matrix<double> A = CréerMatriceA(pointsSpline[0].x, pointsSpline[1].x, pointsSpline[2].x);
        
        coefficients = A.Solve(b);

        for (float i = 0; i <= pointsSpline[2].x; i += 0.5f)
        {
            points.Add(CréerPointSpline(i));
        }
        transform.localPosition = points[current];
    }
    private Vector<double> CréerMatriceB(double y1, double y2, double y3) => Vector<double>.Build.Dense(new[] {y1, y2, y2, y3, 0, 0, 0, 0});
    private Matrix<double> CréerMatriceA(double x1, double x2, double x3)
    {
        return Matrix<double>.Build.DenseOfArray(new[,] {
            
            { Math.Pow(x1, 3), Math.Pow(x1, 2), x1, 1, 0, 0, 0, 0 },
            
            { Math.Pow(x2, 3), Math.Pow(x2, 2), x2, 1, 0, 0, 0, 0 },
            
            { 0, 0, 0, 0, Math.Pow(x2, 3), Math.Pow(x2, 2), x2, 1,},
            
            { 0, 0, 0, 0, Math.Pow(x3, 3), Math.Pow(x3, 2), x3, 1 },
            
            { Math.Pow(x2,2)*3f, x2*2f, 1, 0, -Math.Pow(x2,2)*3f, -(x2*2f), -1, 0},
            
            { x2*6f, 2, 0, 0, -x2*6f, -2, 0, 0},
            
            { x1*6f, 2, 0, 0, 0, 0, 0, 0}, 
            
            { 0, 0, 0, 0, x3*6f, 2, 0, 0} 
        });
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
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[current], vitesse);

            elapsedTime += Time.deltaTime;
            if (transform.localPosition == points[current] && elapsedTime >= 2)
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
