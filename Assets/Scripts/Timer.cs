using System;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public static float timeRemaining = 0;
    public TMP_Text timeText;

    void Update()
    {
        if (GameObject.Find("JoueurLocal").GetComponent<Saut>().nbSauts> 0)
        {
            timeRemaining += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
            timeText.text = time.ToString(@"mm\:ss\:ff");
        }
        else if (GameObject.Find("JoueurLocal").GetComponent<Saut>().nbSauts == 0)
        {
            timeRemaining = 0;
            TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
            timeText.text = time.ToString(@"mm\:ss\:ff");
        }
    }
}