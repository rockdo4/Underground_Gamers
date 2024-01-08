using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f;
    public void Start()
    {
        AdjustCamera();
    }
    public void AdjustCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found.");
            return;
        }
        float currentAspectRatio = (float)Screen.width / Screen.height;

        if (currentAspectRatio < targetAspectRatio)
        {
            float offsetValue = currentAspectRatio / targetAspectRatio;
            float yOffset = (1 - offsetValue) / 2f;

            mainCamera.rect = new Rect(0.0f, yOffset, 1.0f, currentAspectRatio/ targetAspectRatio);
        }
    }
    void OnPreCull()
    {
        GL.Viewport(new Rect(0, 0, Screen.width, Screen.height));
        GL.Clear(true, true, Color.black);
    }
}
