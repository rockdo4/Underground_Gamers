using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoDrawer : MonoBehaviour
{
    public Image PlayerImage;
    public Image TypeIcon;
    public TMP_Text PlayerName;
    public Image Stars;
    public TMP_Text breakThrough;

    public TMP_Text XpVal;
    public Slider XpBar;
    public Transform PlayerCharPos;
    [HideInInspector]
    public GameObject PlayerChar;
    public Image SkillIcon;
    public TMP_Text SkillName;
    public TMP_Text SkillLevel;
    public TMP_Text SkillText;
    public List<TMP_Text> StatsInfo;

    private Player currPlayer;
    private PlayerTable pt;
    private StringTable st;

    private void OnEnable()
    {
        if (pt == null || st == null)
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        if (GamePlayerInfo.instance.usingPlayers == null)
        {
            return;
        }
        if (LobbyUIManager.instance.isUsingPlayer)
        {
            currPlayer = GamePlayerInfo.instance.usingPlayers[LobbyUIManager.instance.PlayerInfoIndex];
        }
        else
        {
            currPlayer = GamePlayerInfo.instance.havePlayers[LobbyUIManager.instance.PlayerInfoIndex];
        }

        PlayerInfo playerInfo = pt.playerDatabase[currPlayer.code];

        PlayerImage.sprite = pt.GetPlayerFullSprite(currPlayer.code);
        PlayerName.text = currPlayer.name;
        breakThrough.text = $"{st.Get("break")}: {currPlayer.breakthrough}";
        XpVal.text = $"{currPlayer.level}/{currPlayer.maxLevel}";
        XpBar.value = currPlayer.xp / currPlayer.maxXp;
        PlayerChar = Instantiate(Resources.Load<GameObject>(Path.Combine("SPUM", $"{currPlayer.code}")), PlayerCharPos);
        PlayerChar.layer = 10;
        PlayerChar.GetComponent<RectTransform>().localScale = Vector3.one*170;
        PlayerChar.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,-57,0);
        StatsInfo[0].text = pt.CalculateCurrStats(playerInfo.hp, currPlayer.level).ToString();
        StatsInfo[1].text = pt.CalculateCurrStats(playerInfo.atk, currPlayer.level).ToString();
        StatsInfo[2].text = pt.CalculateCurrStats(playerInfo.atkRate, currPlayer.level).ToString();
        StatsInfo[3].text = pt.CalculateCurrStats(playerInfo.critical, currPlayer.level).ToString();
        StatsInfo[4].text = pt.CalculateCurrStats(playerInfo.accuracy, currPlayer.level).ToString();
        StatsInfo[5].text = pt.CalculateCurrStats(playerInfo.range, currPlayer.level).ToString();
        StatsInfo[6].text = playerInfo.magazine.ToString();
        StatsInfo[7].text = playerInfo.reloadingSpeed.ToString();
        StatsInfo[8].text = pt.CalculateCurrStats(playerInfo.moveSpeed, currPlayer.level).ToString();
        StatsInfo[9].text = pt.CalculateCurrStats(playerInfo.reactionSpeed, currPlayer.level).ToString();
        TypeIcon.sprite = Resources.Load<Sprite>(Path.Combine("PlayerType", playerInfo.type.ToString()));
    }

    private void OnDisable()
    {
        Destroy(PlayerChar);
        PlayerChar = null;
    }
}
