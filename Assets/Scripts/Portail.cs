using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portail : MonoBehaviour
{
    [SerializeField] GameObject portail;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = portail.transform.position;
    }
}
