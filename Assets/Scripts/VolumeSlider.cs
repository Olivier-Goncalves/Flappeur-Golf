using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume" , 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    public void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("volume" , volumeSlider.value);
    }
}