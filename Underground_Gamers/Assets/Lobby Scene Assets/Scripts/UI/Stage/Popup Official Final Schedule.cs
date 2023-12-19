using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupOfficialFinalSchedule : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] teamNames = new TMP_Text[6];
    [SerializeField]
    private Image[] teamNamesBack = new Image[6];
    [SerializeField]
    private TMP_Text[] teamScores = new TMP_Text[6];
    private void OnEnable()
    {
        for (int i = 0; i < 3; i++)
        {
            int first = GamePlayerInfo.instance.officialFinalMatchResult[i, 0];
            int second = GamePlayerInfo.instance.officialFinalMatchResult[i, 1];

            if (first == second)
            {
                teamNamesBack[i].color = Color.white;
                teamNamesBack[i + 1].color = Color.white;
            }
            else if (first > second)
            {
                teamNamesBack[i].color = Color.green;
                teamNamesBack[i + 1].color = Color.red;
            }
            else
            {
                teamNamesBack[i].color = Color.red;
                teamNamesBack[i + 1].color = Color.green;
            }

            for (int j = 0; j < 2; j++)
            {
                teamNames[(i * 2) + j].text = GamePlayerInfo.instance.officialFinalMatchResultName[i, j];
                teamScores[(i * 2) + j].text = GamePlayerInfo.instance.officialFinalMatchResult[i, j].ToString();
            }
        }
    }
}
