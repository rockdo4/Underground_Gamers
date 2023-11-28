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

        PlayerImage.sprite = pt.playerSprites[currPlayer.code];
        PlayerName.text = currPlayer.name;
        breakThrough.text = $"{st.Get("break")}: {currPlayer.breakthrough}";
        XpVal.text = $"{currPlayer.level}/{currPlayer.maxLevel}";
        XpBar.value = currPlayer.xp / currPlayer.maxXp;
        PlayerChar = Instantiate(Resources.Load<GameObject>(Path.Combine("SPUM", $"{currPlayer.code}")), PlayerCharPos);
        PlayerChar.layer = 10;
        StatsInfo[0].text = playerInfo.hp.ToString();
        StatsInfo[1].text = playerInfo.atk.ToString();
        StatsInfo[2].text = playerInfo.atkRate.ToString();
        StatsInfo[3].text = playerInfo.criticalChance.ToString();
        StatsInfo[4].text = playerInfo.Accuracy.ToString();
        StatsInfo[5].text = playerInfo.range.ToString();
        StatsInfo[6].text = playerInfo.magazine.ToString();
        StatsInfo[7].text = playerInfo.reloadingSpeed.ToString();
        StatsInfo[8].text = playerInfo.moveSpeed.ToString();
        StatsInfo[9].text = playerInfo.reactionSpeed.ToString();
    }

    private void OnDisable()
    {
        Destroy(PlayerChar);
        PlayerChar = null;
    }
}
