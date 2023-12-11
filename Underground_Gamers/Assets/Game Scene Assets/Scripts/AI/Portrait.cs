using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    public GameObject portrait;

    private void Awake()
    {
        if(portrait == null)
        {
            var ai = transform.GetComponent<AIController>();
            if(ai != null)
            {
                SetPortrait(ai.spum);
            }
        }
    }

    public void SetPortrait(SPUM_Prefabs spum)
    {
        var childs = spum.GetComponentsInChildren<Transform>();
        foreach (var child in childs)
        {
            if(child.name == "HeadSet")
            {
                portrait = Instantiate(child.gameObject);
                return;
            }
        }
    }

    public GameObject GetPortrait()
    {
        return portrait;
    }
}
