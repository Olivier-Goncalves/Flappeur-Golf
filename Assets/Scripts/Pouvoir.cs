using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouvoir : MonoBehaviour
{
    private int pouvoirLayer = 15;
    private float elapsedTime;
    [SerializeField] private GameObject fusil;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == pouvoirLayer)
        {
            GetComponent<Jump>().jumpStrength = UnityEngine.Random.Range(0, 2) == 0 ? 25 : 150;
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 16)
        {
            Transform[] position = gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < position.Length; ++i)
            {
                if (position[i].gameObject.name == "EmplacementGun")
                {
                    GameObject nouveauFusil = Instantiate(fusil, position[i].position, Quaternion.identity);
                    nouveauFusil.transform.SetParent(gameObject.transform);
                }
            }
            Destroy(other.gameObject);
        }
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 7.5f)
        {
            elapsedTime = 0;
            GetComponent<Jump>().jumpStrength = 100;
        }
    }
}
