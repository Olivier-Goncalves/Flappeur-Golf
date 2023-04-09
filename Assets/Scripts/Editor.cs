using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Corridors), true)]
public class BoutonRestart : Editor
{
    private Corridors generator;

    private void Awake()
    {
        generator = (Corridors)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Créer Map"))
        {
            generator.Recommencer();
        }
    }
}
