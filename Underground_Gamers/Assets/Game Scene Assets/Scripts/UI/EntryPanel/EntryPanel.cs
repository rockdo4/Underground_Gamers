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
    private List<EntryPlayer> playerList = new List<EntryPlayer>();
    private List<EntryPlayer> entryMembers = new List<EntryPlayer>();
    private List<EntryPlayer> benchMembers = new List<EntryPlayer>();

    public int selectedEntryInedx = 0;
    public int selectedBenchIndex = 0;
    public void ClearPlayerList()
    {
        foreach(EntryPlayer player in playerList)
        {
            player.gameObject.SetActive(false);
        }
        playerList.Clear();
    }

    public void SetActiveEntryMembers(bool isActive)
    {
        foreach (EntryPlayer player in entryMembers)
        {
            player.SetActiveSelectOutline(isActive);
        }
    }    
    public void SetActiveBenchMembers(bool isActive)
    {
        foreach (EntryPlayer player in benchMembers)
        {
            player.SetActiveSelectOutline(isActive);
        }
    }

    public void SetActiveEntryPanel(bool isActive)
    {
        Time.timeScale = 0f;
        gameManager.IsStart = false;
        gameObject.SetActive(isActive);
    }
    public void CreateEntryPlayer(Transform parent, int index, Sprite illustration, string name, int playerHp, int playerAttack, Sprite grade, Sprite type, int level, Sprite codition, int skillLevel)
    {
        EntryPlayer entryPlayer = Instantiate(entryPlayerPrefab, parent);
        entryPlayer.SetInfo(gameManager, index, illustration, name, playerHp, playerAttack, grade, type, level, codition, skillLevel);
        playerList.Add(entryPlayer);

        if (parent == entryScrollView)
        {
            entryPlayer.isEntry = true;
            entryMembers.Add(entryPlayer);
        }
        else
        {
            entryPlayer.isEntry = false;
            benchMembers.Add(entryPlayer);
        }
    }
    public void SetEntryPlayerSlot(Transform parent, int index)
    {
        if (pt == null)
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);

        int code = GamePlayerInfo.instance.GetOfficialPlayer(index - 1).code;
        if (code < 0)
        {
            Debug.Log("Code Error");
            return;
        }
        var playerInfo = pt.GetPlayerInfo(code);
        var player = GamePlayerInfo.instance.GetOfficialPlayer(index - 1);
        foreach (var item in player.training)
        {
            var ti = pt.GetTrainingInfo(item);
            ti.AddStats(playerInfo);
        }
        Sprite illustration = pt.GetPlayerSprite(code);
        var name = GamePlayerInfo.instance.GetOfficialPlayer(index - 1).name;
        int playerHp = (int)pt.CalculateCurrStats(playerInfo.hp, player.level);
        int playerAttack = (int)pt.CalculateCurrStats(playerInfo.atk, player.level);
        var grade = pt.starsSprites[GamePlayerInfo.instance.GetOfficialPlayer(index - 1).grade - 3];
        var type = pt.playerTypeSprites[GamePlayerInfo.instance.GetOfficialPlayer(index - 1).type - 1];
        var level = GamePlayerInfo.instance.GetOfficialPlayer(index - 1).level;
        var condition = GamePlayerInfo.instance.GetOfficialPlayer(index - 1).condition;
        //var skillIcon;
        //var skillName;
        var skillLevel = GamePlayerInfo.instance.GetOfficialPlayer(index - 1).skillLevel;

        // 이 인덱스 생각하기
        CreateEntryPlayer(parent, index, illustration, name, playerHp, playerAttack, grade, type, level, conditionIcon[condition], skillLevel);
    }

    public void SetOriginMemberIndex()
    {
        for(int entryInedx = 1; entryInedx < 6; ++entryInedx)
        {
            GameInfo.instance.entryMembers.Add(entryInedx);
        }        
        for(int benchIndex = 6; benchIndex < 9; ++benchIndex)
        {
            GameInfo.instance.benchMembers.Add(benchIndex);
        }
    }

    public void SetPlayerEntrySlotAndBenchSlot()
    {
        foreach(int entryIndex in GameInfo.instance.entryMembers)
        {
            SetEntryPlayerSlot(entryScrollView, entryIndex);
        }
        foreach(int benchIndex in GameInfo.instance.benchMembers)
        {
            SetEntryPlayerSlot(benchScrollView, benchIndex);
        }
    }

    public void SetBattleLayoutForge()
    {
        GameInfo.instance.StartGame();

        // 엔트리 결정 후 라인 지정 가기전에 해줘야 할 것들
        gameManager.entryPanel.SetActiveEntryPanel(false);

        GameInfo.instance.SetEntryPlayer(GameInfo.instance.entryMembers);
        GameInfo.instance.MakePlayers();
        gameManager.settingAIID.SetAIIDs();
        //gameManager.entryManager.RefreshSelectLineButton();

        // 라인 지정 UI
        gameManager.entryManager.ResetBattleLayoutForge();
        // 라인 지정 UI On
        gameManager.battleLayoutForge.SetActiveBattleLayoutForge(true);
    }
}
