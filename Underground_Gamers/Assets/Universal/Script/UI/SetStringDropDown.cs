using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetStringDropDown : MonoBehaviour
{
    public List<string> textId;
    void Start()
    {
        var dropdowns = GetComponent<TMP_Dropdown>().options;
        int count = 0;
        foreach (var dropdown in dropdowns)
        {
            dropdown.text = DataTableManager.instance.Get<StringTable>(DataType.String).Get(textId[count++]);
            if (count == dropdowns.Count)
            {
                count = 0;
            }
        }
    }
}
