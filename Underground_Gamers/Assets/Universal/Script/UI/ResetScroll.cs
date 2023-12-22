using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void ResetScrollView()
    {
        if(scrollRect.horizontal)
            scrollRect.normalizedPosition = new Vector2(0, 1);

        if(scrollRect.vertical)
            scrollRect.normalizedPosition = new Vector2(1, 0);
    }
}
