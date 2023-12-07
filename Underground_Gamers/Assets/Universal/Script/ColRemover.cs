using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColRemover : MonoBehaviour
{
    public void RemoveCol()
    {
        var objs = GameObject.FindObjectsOfType<MeshCollider>();
        foreach (var obj in objs)
        {
            Destroy(obj);
        }
    }
}
