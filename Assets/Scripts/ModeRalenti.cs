using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeRalenti : MonoBehaviour
{
    public float valeurRalenti=0.1f;
    private float d�butTempsArrang�;
    public float temps = 0f;
    private bool ralentiFini = false;
    private float valeurTempsNormal;
    private void D�butRalenti()
    {
        Time.timeScale = valeurRalenti;
        Time.fixedDeltaTime = d�butTempsArrang� * valeurRalenti;
    }

    private void Arr�tRalenti()
    {
        Time.timeScale = valeurTempsNormal;
        Time.fixedDeltaTime = d�butTempsArrang�;
    }
    void Awake()
    {
        valeurTempsNormal = Time.timeScale;
        d�butTempsArrang� = Time.fixedDeltaTime;
    }

    void Update()
    { 
        if(Input.GetKey(KeyCode.Mouse1))
        {
            if (ralentiFini == false) 
            { 
                D�butRalenti(); 
                temps += (Time.deltaTime/valeurRalenti);
                Debug.Log(temps);
                if (temps >= 5) 
                { 
                    Arr�tRalenti(); 
                    ralentiFini = true;
                } 
            } 
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Arr�tRalenti();
        }
    }

}
