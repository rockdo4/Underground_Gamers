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
        var st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        var str = DataTableManager.instance.Get<StringTable>(DataType.String);
        if(GamePlayerInfo.instance.officialWeekNum <= 7)
        {
            title.text = str.Get("playoff_fail");
        }
        else if(GamePlayerInfo.instance.officialWeekNum <= 10)
        {
            title.text = str.Get($"grade{12 - GamePlayerInfo.instance.officialWeekNum}");
        }
        else
        {
            title.text = str.Get("win_official");
        }

        var rewards = st.GetOfficialRewards();
        for (int i = 0; i < counts.Length; i++)
        {
            counts[i].text = rewards[i].ToString();
        }
        GamePlayerInfo.instance.AddMoney(rewards[4], rewards[5], 0);
        GamePlayerInfo.instance.GetXpItems(rewards[0], rewards[1], rewards[2], rewards[3]);
    }
}
