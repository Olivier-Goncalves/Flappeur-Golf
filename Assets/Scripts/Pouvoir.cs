using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouvoir : MonoBehaviour
{
    private int pouvoirLayer = 15;
    private float elapsedTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == pouvoirLayer)
        {
            GetComponent<Jump>().jumpStrength = UnityEngine.Random.Range(0, 2) == 0 ? 50 : 150;
            Destroy(other.gameObject);
        }
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 7.5f)
        {
            elapsedTime = 0;
            GetComponent<Jump>().jumpStrength = 100;
        }
    }
}
