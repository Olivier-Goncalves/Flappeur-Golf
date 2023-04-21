using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeRalenti : MonoBehaviour
{
    public float valeurRalenti=0.2f;
    private float débutTempsArrangé;
    public float tempsLimite = 5f;
    private float débutTemps;
    private void DébutRalenti()
    {
        Time.timeScale = valeurRalenti;
        Time.fixedDeltaTime = débutTempsArrangé * valeurRalenti;
    }

    private void ArrêtRalenti()
    {
        Time.timeScale = débutTemps;
        Time.fixedDeltaTime = débutTempsArrangé;
    }
    void Awake()
    {
        débutTemps = Time.timeScale;
        débutTempsArrangé = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (tempsLimite >= 0)
            {
                tempsLimite = -Time.fixedTime; 
                DébutRalenti();
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ArrêtRalenti();
        }
    }

}
