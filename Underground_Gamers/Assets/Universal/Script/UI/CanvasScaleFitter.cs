using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScaleFitter : MonoBehaviour
{
    void Start()
    {
        if (Screen.width / Screen.height < 16f / 9f)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
    }
}
