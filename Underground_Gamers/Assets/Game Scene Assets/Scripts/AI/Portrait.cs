using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    public GameObject portrait;
    private float scale = 100f;

    private void Awake()
    {
        if (portrait == null)
        {
            var ai = transform.GetComponent<AIController>();
            if (ai != null && ai.teamIdentity == null)
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
                portrait.SetActive(false);
                return;
            }
        }
    }

    public GameObject GetPortrait()
    {
        return portrait;
    }
}
