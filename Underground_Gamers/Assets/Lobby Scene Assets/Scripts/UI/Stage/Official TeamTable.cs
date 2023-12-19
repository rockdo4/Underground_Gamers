using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficialTeamTable : MonoBehaviour
{
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private TMP_Text[] infos = new TMP_Text[7];

    public void SetInfos(OfficialTeamData officialTeamData, int index)
    {
        infos[0].text = (index + 1).ToString();
        infos[1].text = officialTeamData.name.ToString();
        infos[2].text = officialTeamData.win.ToString();
        infos[3].text = officialTeamData.lose.ToString();
        infos[4].text = officialTeamData.setWin.ToString();
        infos[5].text = officialTeamData.setLose.ToString();
        int setValue = officialTeamData.setWin - officialTeamData.setLose;
        if (setValue > 0)
        {
            infos[6].text = "+";
        }
        else
        {
            infos[6].text = "";
        }
        infos[6].text += setValue.ToString();
        if(officialTeamData.isPlayer)
        {
            selectedImage.color = Color.yellow;
        }
        else
        {
            selectedImage.color = Color.white;
        }
    }
}
