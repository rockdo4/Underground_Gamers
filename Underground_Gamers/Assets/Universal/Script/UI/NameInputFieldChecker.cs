using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputFieldChecker : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInputBox;
    [SerializeField]
    private Button button;
    [SerializeField]
    private int count = 8;

    public void CheckInput()
    {
        if (nameInputBox.text.Length == 0 ||
            nameInputBox.text.Length > count) 
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void InputStreamString(string newText)
    {
        if (newText.Length > count)
        {
            nameInputBox.text = newText.Substring(0, count);
        }
    }

}
