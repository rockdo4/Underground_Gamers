using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAccendingText : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    private TMP_Text text;
    private StringTable st;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    public void ToggleCheck()
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        if (toggle.isOn)
        {
            text.text = st.Get("descending");
        }
        else
        {
            text.text = st.Get("ascending");
        }
    }

}
