using UnityEngine;
using UnityEngine.UI;

public class OverlayLetterboxController : MonoBehaviour
{
    public RectTransform uiElement;

    void Start()
    {
        ResizeUI();
    }

    void Update()
    {
        // Update the UI if the screen size changes
        if (Screen.width != uiElement.rect.width || Screen.height != uiElement.rect.height)
        {
            ResizeUI();
        }
    }

    void ResizeUI()
    {
        // Get the ratio of the current screen size to the reference resolution
        float ratioWidth = (float)Screen.width / GetComponent<CanvasScaler>().referenceResolution.x;
        float ratioHeight = (float)Screen.height / GetComponent<CanvasScaler>().referenceResolution.y;

        // Scale the UI element based on the screen's ratio
        uiElement.localScale = new Vector3(ratioWidth, ratioHeight, 1f);
    }
}

