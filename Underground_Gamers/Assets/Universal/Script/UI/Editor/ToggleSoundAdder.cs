using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(ToggleSound))]
public class ToggleSoundAdder : MonoBehaviour
{
    public void AddToggleSound()
    {
        var objs = GameObject.FindObjectsOfType<Toggle>(true);
        foreach (var obj in objs)
        {
            if (obj.GetComponent<EventTrigger>() == null)
            {
                obj.AddComponent<ToggleSound>();
                obj.AddComponent<EventTrigger>();
            }
        }
    }
}
