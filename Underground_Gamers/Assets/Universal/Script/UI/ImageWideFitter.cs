using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageWideFitter : MonoBehaviour
{
    private Image imageToResize;
    void Start()
    {
        imageToResize = GetComponent<Image>();
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        RectTransform imageRect = imageToResize.rectTransform;

        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        float imageWidth = imageToResize.sprite.rect.width;
        float imageHeight = imageToResize.sprite.rect.height;

        float widthRatio = parentWidth / imageWidth;
        float heightRatio = parentHeight / imageHeight;

        float ratio = Mathf.Max(widthRatio, heightRatio);
        float newWidth = imageWidth * ratio;
        float newHeight = imageHeight * ratio;

        imageRect.sizeDelta = new Vector2(newWidth, newHeight);
        imageRect.position = parentRect.position;
        imageRect.anchoredPosition3D = new Vector2(0,0);
    }
}
