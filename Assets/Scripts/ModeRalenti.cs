using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeRalenti : MonoBehaviour
{
    public float valeurRalenti=0.2f;
    private float d�butTempsArrang�;
    public float tempsLimite = 5f;
    private float d�butTemps;
    private void D�butRalenti()
    {
        Time.timeScale = valeurRalenti;
        Time.fixedDeltaTime = d�butTempsArrang� * valeurRalenti;
    }

    private void Arr�tRalenti()
    {
        Time.timeScale = d�butTemps;
        Time.fixedDeltaTime = d�butTempsArrang�;
    }
    void Awake()
    {
        d�butTemps = Time.timeScale;
        d�butTempsArrang� = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (tempsLimite >= 0)
            {
                tempsLimite = -Time.fixedTime; 
                D�butRalenti();
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Arr�tRalenti();
        }
    }

}
