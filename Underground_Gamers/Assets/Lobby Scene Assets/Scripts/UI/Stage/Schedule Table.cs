using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleTable : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] teamNames = new TMP_Text[8];   
    [SerializeField]
    private Image[] teamNamesPanel = new Image[8];
    [SerializeField]
    private TMP_Text[] scores = new TMP_Text[8];
    [SerializeField]
    private Image markCurrent;

    [SerializeField]
    private int index = 0;

    public void SetText()
    {
        for (int i = 0; i < 8; i++)
        {
            OfficialTeamData currentData = GamePlayerInfo.instance.officialTeamDatas[GamePlayerInfo.instance.officialMatchInfo[index, i]];
            teamNames[i].text = currentData.name;
            scores[i].text = GamePlayerInfo.instance.officialMatchResult[index, i].ToString();
            if (currentData.isPlayer)
            {
                teamNamesPanel[i].color = Color.yellow;
            }
            else
            {
                teamNamesPanel[i].color = Color.white;
            }
        }

        if (index == GamePlayerInfo.instance.officialWeekNum)
        {
            markCurrent.color = Color.blue;
        }
        else
        {
            markCurrent.color = Color.white;
        }
    }
}
