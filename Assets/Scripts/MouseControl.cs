using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Fait par: Guillaume Flamand & Louis-Félix Bouvrette

public class MouseControl : MonoBehaviour
{
    [SerializeField] private float sensitivité = 1;
    [SerializeField] private Transform cameraJoueur;
    [SerializeField] private GestionJeuSolo gestionnaireJeu;
    private float camDistance;
    private Vector2 distanceCameraMinMax = new Vector2(0.4f, 7f);
    private Vector3 directionCamera;
    private float x;
    private float y;
    private float coussin = 0.2f;
    private void Start()
    {
        directionCamera = cameraJoueur.transform.localPosition.normalized;
        camDistance = distanceCameraMinMax.y;
    }
    
    void Update()
    {
        if (!gestionnaireJeu.pause)
        {
            x += sensitivité * Input.GetAxis("Mouse X");
            y -= sensitivité * Input.GetAxis("Mouse Y");
        }
        Cursor.lockState = CursorLockMode.Confined;
        transform.eulerAngles = new Vector3(y, x, 0f);
        AjusterPositionCamera();
    }
    // source: https://www.youtube.com/watch?v=vpn8CbPpvlU
    public void AjusterPositionCamera()
    {
        Vector3 positionCameraVoulu = transform.TransformPoint(directionCamera * distanceCameraMinMax.y);
        RaycastHit hit;
        if (Physics.Linecast(transform.position, positionCameraVoulu, out hit))
        {
            camDistance = Mathf.Clamp(hit.distance - coussin, distanceCameraMinMax.x , distanceCameraMinMax.y);
        }
        else
        {
            camDistance = distanceCameraMinMax.y;
        }
        cameraJoueur.localPosition = directionCamera * camDistance;
    }
}
