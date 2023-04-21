using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeRalenti : MonoBehaviour
{
    public float valeurRalenti=0.1f;
    private float débutTempsArrangé;
    public float temps = 0f;
    private bool ralentiFini = false;
    private float valeurTempsNormal;
    private void DébutRalenti()
    {
        Time.timeScale = valeurRalenti;
        Time.fixedDeltaTime = débutTempsArrangé * valeurRalenti;
    }

    private void ArrêtRalenti()
    {
        Time.timeScale = valeurTempsNormal;
        Time.fixedDeltaTime = débutTempsArrangé;
    }
    void Awake()
    {
        valeurTempsNormal = Time.timeScale;
        débutTempsArrangé = Time.fixedDeltaTime;
    }

    void Update()
    { 
        if(Input.GetKey(KeyCode.Mouse1))
        {
            if (ralentiFini == false) 
            { 
                DébutRalenti(); 
                temps += (Time.deltaTime/valeurRalenti);
                Debug.Log(temps);
                if (temps >= 5) 
                { 
                    ArrêtRalenti(); 
                    ralentiFini = true;
                } 
            } 
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ArrêtRalenti();
        }
    }

}
