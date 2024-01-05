using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToggleSoundAdder))]
public class AddToggleSound : Editor
{
    public override void OnInspectorGUI()
    {
        ToggleSoundAdder toggleSoundAdder = (ToggleSoundAdder)target;

        if (GUILayout.Button("Add Toggle Sound"))
        {
            toggleSoundAdder.AddToggleSound();
        }
    }
}
