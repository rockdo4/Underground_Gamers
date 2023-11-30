using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetString : MonoBehaviour
{
    public string textId;
    void Start()
    {
        GetComponent<TMP_Text>().text = DataTableManager.instance.Get<StringTable>(DataType.String).Get(textId);
    }
}
