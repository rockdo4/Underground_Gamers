using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleColorChange : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private Toggle toggle;
    public Image checkmark;
    public Image background;

    public Color pressedColor;
    public Color normalColor;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(toggle.isOn)
        {
            checkmark.color = pressedColor;
        }
        else
        {
            background.color = pressedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toggle.isOn)
        {
            checkmark.color = normalColor;
        }
        else
        {
            background.color = normalColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (toggle.isOn)
        {
            checkmark.color = normalColor;
        }
        else
        {
            background.color = normalColor;
        }
    }
}
