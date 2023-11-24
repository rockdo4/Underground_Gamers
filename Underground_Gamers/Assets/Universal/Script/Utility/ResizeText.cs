using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoFontSize : MonoBehaviour
{
    private TMP_Text textComponent;
    public float maxFontSize = 30f;
    public float minFontSize = 10f;
    public float maxSizeWidth = 200f;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        ResizeText();
    }

    void ResizeText()
    {
        if (textComponent == null)
        {
            Debug.LogError("Text component not assigned!");
            return;
        }

        float fontSize = textComponent.fontSize;

        float textWidth = textComponent.preferredWidth;

        while (textWidth > maxSizeWidth && fontSize > minFontSize)
        {
            fontSize--;
            textComponent.fontSize = fontSize;
            textWidth = textComponent.preferredWidth;
        }

        while (textWidth < maxSizeWidth && fontSize < maxFontSize)
        {
            fontSize++;
            textComponent.fontSize = fontSize;
            textWidth = textComponent.preferredWidth;
        }
    }
}

