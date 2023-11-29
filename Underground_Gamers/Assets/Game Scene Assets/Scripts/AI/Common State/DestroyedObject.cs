using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObject : MonoBehaviour, IDestroyable
{
    public void DestoryObject()
    {
        CharacterStatus status = GetComponent<CharacterStatus>();
        //Destroy(gameObject);
        if(status != null)
        {
            TargetEventBus.Publish(status);
        }
        gameObject.SetActive(false);
    }
}
