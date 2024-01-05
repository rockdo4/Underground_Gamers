using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonSoundAdder))]
public class AddButtonSound : Editor
{
    public override void OnInspectorGUI()
    {
        ButtonSoundAdder buttonSoundAdder = (ButtonSoundAdder)target;

        if (GUILayout.Button("Add Button Sound"))
        {
            buttonSoundAdder.AddButtonSound();
        }
    }
}
