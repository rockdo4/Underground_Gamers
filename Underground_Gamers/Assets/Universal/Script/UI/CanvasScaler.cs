using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScaleSetter : MonoBehaviour
{
    public CanvasScaler scaler;
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.width / Screen.height < 1.5f)
        {
            scaler.matchWidthOrHeight = 0f;
        }
        else
        {
            scaler.matchWidthOrHeight = 1f;
        }
    }
}
