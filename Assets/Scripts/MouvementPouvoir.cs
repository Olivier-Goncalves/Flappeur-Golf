using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementPouvoir : MonoBehaviour
{
    private Vector3 posOffset;
    private Vector3 tempPos;
    
    void Start () 
    {
        posOffset = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI) * 0.8f;

        transform.position = tempPos;
    }
}
