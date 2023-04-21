using System;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

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
    public void ChangeVolume(float valeur)
    {
        AudioListener.volume = valeur;
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