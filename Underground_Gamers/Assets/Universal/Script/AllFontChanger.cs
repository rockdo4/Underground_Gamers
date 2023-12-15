using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllFontChanger : MonoBehaviour
{
    public TMP_FontAsset fontAsset;
    public void SetFonts()
    {
        var objs = GameObject.FindObjectsOfType<TMP_Text>(true);
        //var boxs = GameObject.FindObjectsOfType<BoxCollider>(true);
        foreach (var obj in objs)
        {
            obj.font = fontAsset;
        }

        //foreach (var obj in boxs)
        //{
        //    DestroyImmediate(obj);
        //}
    }
}