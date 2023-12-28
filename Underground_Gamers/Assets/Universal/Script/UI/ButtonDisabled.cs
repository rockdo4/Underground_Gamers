using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabled : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private Button button;

    public void OnEnable()
    {
        CheckEnable();
    }
    public void CheckEnable()
    {
        if (button.interactable)
        {
            text.color = Color.white;
        }
        else
        {
            text.color = Color.gray;
        }
    }
}
