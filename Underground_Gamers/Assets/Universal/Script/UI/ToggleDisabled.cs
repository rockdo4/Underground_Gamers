using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDisabled : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private GameObject disableImage;

    public void InteractableCheck()
    {
        if (toggle.interactable)
        {
            disableImage.SetActive(false);
        }
        else
        {
            disableImage.SetActive(true);
        }
    }
}
