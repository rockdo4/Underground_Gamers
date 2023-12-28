using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneCanvas : MonoBehaviour
{
    public RectTransform canvasRect;
    Rect safeArea;
    Vector2 minAnchor;
    Vector2 maxAnchor;

    void Awake()
    {
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        canvasRect.anchorMin = minAnchor;
        canvasRect.anchorMax = maxAnchor;
    }
}
