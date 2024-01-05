using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ButtonSound))]
public class ButtonSoundAdder : MonoBehaviour
{
    public void AddButtonSound()
    {
        var objs = GameObject.FindObjectsOfType<Button>(true);
        foreach (var obj in objs)
        {
            if (obj.GetComponent<ButtonSound>() == null)
                obj.AddComponent<ButtonSound>();
        }
    }
}
