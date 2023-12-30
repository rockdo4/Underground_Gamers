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

    public void CheckInput()
    {
        if (nameInputBox.text.Length == 0 ||
            nameInputBox.text.Length > 8) 
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
