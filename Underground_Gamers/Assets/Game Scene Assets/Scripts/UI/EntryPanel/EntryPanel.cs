using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPanel : MonoBehaviour
{
    public GameManager gameManager;
    public Transform entryScrollView;
    public Transform benchScrollView;

    public EntryPlayer entryPlayerPrefab;

    private PlayerTable pt;

    public Sprite[] conditionIcon = new Sprite[5];

    public void SetActiveEntryPanel(bool isActive)
    {
        Time.timeScale = 0f;
        gameManager.IsStart = false;
        gameObject.SetActive(isActive);
    }
    public void CreateEntryPlayer(Transform parent, int index, Sprite illustration, string name, int playerHp, int playerAttack, Sprite grade, Sprite type, int level, Sprite codition, int skillLevel)
    {
        EntryPlayer entryPlayer = Instantiate(entryPlayerPrefab, parent);
        entryPlayer.SetInfo(index, illustration, name, playerHp, playerAttack, grade, type, level, codition, skillLevel);
    }
    public void SetEntryPlayerSlot(Transform parent, int index)
    {
        if (pt == null)
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);

        int code = GamePlayerInfo.instance.GetOfficialPlayer(index).code;
        if (code < 0)
        {
            Debug.Log("Code Error");
            return;
        }
        var playerInfo = pt.GetPlayerInfo(code);
        var player = GamePlayerInfo.instance.GetOfficialPlayer(index);
        foreach (var item in player.training)
        {
            var ti = pt.GetTrainingInfo(item);
            ti.AddStats(playerInfo);
        }
        Sprite illustration = pt.GetPlayerSprite(code);
        var name = GamePlayerInfo.instance.GetOfficialPlayer(index).name;
        int playerHp = (int)pt.CalculateCurrStats(playerInfo.hp, player.level);
        int playerAttack = (int)pt.CalculateCurrStats(playerInfo.atk, player.level);
        var grade = pt.starsSprites[GamePlayerInfo.instance.GetOfficialPlayer(index).grade - 3];
        var type = pt.playerTypeSprites[GamePlayerInfo.instance.GetOfficialPlayer(index).type - 1];
        var level = GamePlayerInfo.instance.GetOfficialPlayer(index).level;
        var condition = GamePlayerInfo.instance.GetOfficialPlayer(index).condition;
        //var skillIcon;
        //var skillName;
        var skillLevel = GamePlayerInfo.instance.GetOfficialPlayer(index).skillLevel;
        CreateEntryPlayer(parent, index, illustration, name, playerHp, playerAttack, grade, type, level, conditionIcon[condition], skillLevel);
    }

    public void SetPlayerEntrySlotAndBenchSlot()
    {
        for (int i = 0; i < GamePlayerInfo.instance.officialPlayers.Count; ++i)
        {
            if (i < 5)
                SetEntryPlayerSlot(entryScrollView, i);
            else
                SetEntryPlayerSlot(benchScrollView, i);
        }
    }

    public void SetBattleLayoutForge()
    {
        GameInfo.instance.StartGame();

        // 엔트리 결정 후 라인 지정 가기전에 해줘야 할 것들
        gameManager.entryPanel.SetActiveEntryPanel(false);

        int[] temp = new int[5]
        {
            0, 1, 2, 3, 4
        };
        GameInfo.instance.SetEntryPlayer(temp);
        GameInfo.instance.MakePlayers();
        gameManager.settingAIID.SetAIIDs();
        //gameManager.entryManager.RefreshSelectLineButton();

        // 라인 지정 UI
        gameManager.entryManager.ResetBattleLayoutForge();
        // 라인 지정 UI On
        gameManager.battleLayoutForge.SetActiveBattleLayoutForge(true);
    }
}
