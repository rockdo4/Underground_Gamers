using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObject : MonoBehaviour, IDestroyable
{
    public void DestoryObject()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
