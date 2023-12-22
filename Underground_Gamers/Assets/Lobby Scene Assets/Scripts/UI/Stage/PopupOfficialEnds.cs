using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupOfficialEnds : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text[] counts = new TMP_Text[6];

    private void OnEnable()
    {
        var st = DataTableManager.instance.Get<StringTable>(DataType.String);
        if(GamePlayerInfo.instance.officialWeekNum <= 7)
        {
            title.text = st.Get("playoff_fail");
        }
        else if(GamePlayerInfo.instance.officialWeekNum < 10)
        {
            title.text = st.Get($"grade{11 - GamePlayerInfo.instance.officialWeekNum}");
        }
        else
        {
            title.text = st.Get("win_official");
        }
    }
}
