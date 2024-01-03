using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficialPlayerTable : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TMP_Text[] infos = new TMP_Text[7];
    [SerializeField]
    private int playerIndex;
    [SerializeField]
    private GameObject popupPlayerDetailInfo;
    [SerializeField]
    private Image[] popupImages = new Image[4];
    [SerializeField]
    private TMP_Text[] popupText = new TMP_Text[18];


    public void SetInfos(OfficialPlayerData officialPlayerData, int index)
    {
        playerIndex = index;
        button.onClick.RemoveAllListeners();
        Player current = GamePlayerInfo.instance.GetOfficialPlayer(playerIndex);
        if (current.code >= 0)
        {
            image.color = Color.white;
            image.sprite = DataTableManager.instance.Get<PlayerTable>(DataType.Player).GetPlayerSprite(current.code);
            infos[0].text = current.name;
            infos[1].text = officialPlayerData.playCount.ToString();
            infos[2].text = officialPlayerData.kill.ToString();
            infos[3].text = officialPlayerData.death.ToString();
            if (officialPlayerData.death != 0f)
            {
                infos[4].text = ((float)officialPlayerData.kill / officialPlayerData.death).ToString("F1");
            }
            else
            {
                infos[4].text = officialPlayerData.kill.ToString();
            }
            if (officialPlayerData.playCount != 0f)
            {
                infos[5].text = ((float)officialPlayerData.totalDamage / officialPlayerData.playCount).ToString("F1");
            }
            else
            {
                infos[5].text = officialPlayerData.totalDamage.ToString();
            }
            infos[6].text = officialPlayerData.totalDamage.ToString();
            button.onClick.AddListener(OpenInfoWindow);
        }
        else
        {
            image.color = Color.clear;
            foreach (var item in infos)
            {
                item.text = "-";
            }
        }
    }

    public void OpenInfoWindow()
    {
        Player currPlayer = GamePlayerInfo.instance.GetOfficialPlayer(playerIndex);
        PlayerTable pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        StringTable st = DataTableManager.instance.Get<StringTable>(DataType.String);
        PlayerInfo currPlayerInfo = pt.GetPlayerInfo(currPlayer.code);
        SkillInfo skillInfo = pt.GetSkillInfo(currPlayerInfo.UniqueSkillCode);
        foreach (var item in currPlayer.training)
        {
            var ti = pt.GetTrainingInfo(item);
            ti.AddStats(currPlayerInfo);
        }

        if (currPlayer != null && currPlayer.code >= 0)
        {
            popupPlayerDetailInfo.SetActive(true);
            popupImages[0].sprite = image.sprite;
            popupImages[1].sprite = pt.playerTypeSprites[currPlayer.type - 1];
            popupImages[2].sprite = pt.starsSprites[currPlayer.grade - 3];
            popupImages[3].sprite = skillInfo.icon;

            popupText[0].text = currPlayer.name;
            popupText[1].text = st.Get($"type{currPlayer.type}");
            popupText[2].text = st.Get("level") + $" : {currPlayer.level}";
            popupText[3].text = st.Get("break") + $" : {currPlayer.breakthrough}";
            popupText[4].text = st.Get("skill_level") + $" : {currPlayer.skillLevel}";
            popupText[5].text = st.Get(skillInfo.name.ToString());
            popupText[6].text = $"{pt.CalculateCurrStats(currPlayerInfo.hp, currPlayer.level).ToString("F0")}";
            popupText[7].text = $"{pt.CalculateCurrStats(currPlayerInfo.atk, currPlayer.level).ToString("F0")}";
            popupText[8].text = $"{pt.CalculateCurrStats(currPlayerInfo.atkRate, currPlayer.level).ToString("F1")}";
            popupText[9].text = $"{pt.CalculateCurrStats(currPlayerInfo.moveSpeed, currPlayer.level).ToString("F0")}";
            popupText[10].text = $"{pt.CalculateCurrStats(currPlayerInfo.sight, currPlayer.level).ToString("F0")}";
            popupText[11].text = $"{pt.CalculateCurrStats(currPlayerInfo.range, currPlayer.level).ToString("F0")}";
            popupText[12].text = $"{pt.CalculateCurrStats(currPlayerInfo.detectionRange, currPlayer.level).ToString("F0")}";
            popupText[13].text = $"{pt.CalculateCurrStats(currPlayerInfo.critical, currPlayer.level).ToString("F1")}";
            popupText[14].text = $"{currPlayerInfo.magazine.ToString("F0")}";
            popupText[15].text = $"{currPlayerInfo.reloadingSpeed.ToString("F0")}";
            popupText[16].text = $"{pt.CalculateCurrStats(currPlayerInfo.accuracy, currPlayer.level).ToString("F0")}";
            popupText[17].text = $"{pt.CalculateCurrStats(currPlayerInfo.reactionSpeed, currPlayer.level).ToString("F0")}";
        }
    }
}
