using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ColRemover))]
public class RemoveMeshColider : Editor
{
    public override void OnInspectorGUI()
    {
        ColRemover colRemover = (ColRemover)target;

        if (GUILayout.Button("Remove Colliders"))
        {
            colRemover.RemoveCol();
        }
    }
}
