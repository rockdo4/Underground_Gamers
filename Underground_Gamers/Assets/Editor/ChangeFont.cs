using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(AllFontChanger))]
public class ChangeFont : Editor
{
    public override void OnInspectorGUI()
    {
        AllFontChanger colRemover = (AllFontChanger)target;

        if (GUILayout.Button("change Fonts"))
        {
            colRemover.SetFonts();
        }
    }
}
