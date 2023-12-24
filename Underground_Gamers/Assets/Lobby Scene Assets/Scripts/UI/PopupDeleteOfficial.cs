using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupDeleteOfficial : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerNames;
    private StringTable stringTable;
    public void ActiveWarning(List<string> names)
    {
        if (stringTable == null)
        {
            stringTable = DataTableManager.instance.Get<StringTable>(DataType.String);
        }

        gameObject.SetActive(true);
        playerNames.text = stringTable.Get("warninig_delete_official2");
        if (names.Count > 0)
        {
            playerNames.text += names[0];
        }

        for (int i = 1; i < names.Count; i++)
        {
            playerNames.text += ", " + names[i];
        }
    }
}
