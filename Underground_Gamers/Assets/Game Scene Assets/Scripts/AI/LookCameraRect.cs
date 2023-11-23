using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraRect : MonoBehaviour
{
    RectTransform rt;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
    public void Update()
    {
        rt.rotation = Camera.main.transform.rotation;
    }
}
