using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxController : MonoBehaviour
{
    public Camera cam;
    public float fixedAspectRatio;
    void Start()
    {
        float targetAspect = 16f / 9f;
        int width = Screen.width;
        int height = Mathf.RoundToInt(width / targetAspect);

        Screen.SetResolution(width, height, true);
    }
    void OnPreCull() => GL.Clear(true, true, Color.black);
}
