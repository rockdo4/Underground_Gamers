using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteTextOnDisableInputBox : MonoBehaviour
{
    private void OnDisable()
    {
        GetComponent<TMP_InputField>().text = string.Empty;
    }
}
