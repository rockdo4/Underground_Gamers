using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColRemover : MonoBehaviour
{
    public void RemoveCol()
    {
        var objs = GameObject.FindObjectsOfType<MeshCollider>(true);
        //var boxs = GameObject.FindObjectsOfType<BoxCollider>(true);
        foreach (var obj in objs)
        {
            DestroyImmediate(obj);
        }        
        
        //foreach (var obj in boxs)
        //{
        //    DestroyImmediate(obj);
        //}
    }
}