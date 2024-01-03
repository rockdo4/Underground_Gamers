using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EntryPanel : MonoBehaviour
{
    public GameManager gameManager;
    public Transform entryScrollView;
    public Transform benchScrollView;

    public EntryPlayer entryPlayerPrefab;

    private PlayerTable pt;

    public Sprite[] conditionIcon = new Sprite[5];
    private List<EntryPlayer> entryMembers = new List<EntryPlayer>();
    private List<EntryPlayer> benchMembers = new List<EntryPlayer>();

    public EntryPlayer selectedEntryMember;
    public EntryPlayer selectedBenchMember;

    public TextMeshProUGUI pickEntryMemberText;
    public TextMeshProUGUI entryMemberText;
    public TextMeshProUGUI benchMemberText;
    public TextMeshProUGUI okButtonText;
    public TextMeshProUGUI giveUpButtonText;


    private void Start()
    {
        pickEntryMemberText.text = gameManager.str.Get("pick entry member");
        entryMemberText.text = gameManager.str.Get("entry member");
        benchMemberText.text = gameManager.str.Get("bench member");
        okButtonText.text = gameManager.str.Get("ok");
        giveUpButtonText.text = gameManager.str.Get("give up");
    }

    public void SetConditionIcon()
    {
        foreach(var member in entryMembers)
        {
            member.SetConditionIcon(conditionIcon[GamePlayerInfo.instance.GetOfficialPlayer(member.Index).condition]);
        }         
        
        foreach(var member in benchMembers)
        {
            member.SetConditionIcon(conditionIcon[GamePlayerInfo.instance.GetOfficialPlayer(member.Index).condition]);
        }
    }

    public void SwapEntryMember()
    {
        if (selectedEntryMember != null && selectedBenchMember != null)
        {
            // UI 선택 해제
            selectedEntryMember.SetActiveSelectOutline(false);
            selectedBenchMember.SetActiveSelectOutline(false);

            // 서호 List교환 후 / 실제 정보 Index도 교환해야함
            selectedEntryMember.transform.SetParent(benchScrollView);
            selectedBenchMember.transform.SetParent(entryScrollView);

            // 정렬 필요
            SortMembers(benchScrollView);
            SortMembers(entryScrollView);

            selectedEntryMember.isEntry = false;
            selectedBenchMember.isEntry = true;

            entryMembers.Remove(selectedEntryMember);
            benchMembers.Add(selectedEntryMember);
            benchMembers.Remove(selectedBenchMember);
            entryMembers.Add(selectedBenchMember);

            // 비워주기
            selectedEntryMember = null;
            selectedBenchMember = null;
        }
    }

    public void SortMembers(Transform parent)
    {
        var childs = parent.GetComponentsInChildren<EntryPlayer>();
        Array.Sort(childs, CompareByIndex);

        foreach (var child in childs)
        {
            int index = child.Index;
            child.transform.SetSiblingIndex(index);
        }

    }

    private int CompareByIndex(EntryPlayer a, EntryPlayer b)
    {
        return a.Index.CompareTo(b.Index);
    }

    public void SetActiveMemberOutlines(bool isEntry, bool isActive)
    {
        if (isEntry)
        {
            SetActiveEntryMemberOutlines(isActive);
        }
        else
        {
            SetActiveBenchMemberOutlines(isActive);
        }
    }

    public void SetActiveEntryMemberOutlines(bool isActive)
    {
        foreach (EntryPlayer player in entryMembers)
        {
            player.SetActiveSelectOutline(isActive);
        }
    }
    public void SetActiveBenchMemberOutlines(bool isActive)
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
    public void CreateEntryPlayer(Transform parent, int index, Sprite illustration, string name, int playerHp, 
        int playerAttack, int grade, Sprite type, int level, Sprite codition, int skillLevel, Sprite skillIcon, string skillName)
    {
        EntryPlayer entryPlayer = Instantiate(entryPlayerPrefab, parent);
        entryPlayer.SetInfo(gameManager, index, illustration, name, playerHp, playerAttack, grade, type, level, codition, skillLevel, skillIcon, skillName);

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

        int code = GamePlayerInfo.instance.GetOfficialPlayer(index).code;
        if (code < 0)
        {
            Debug.Log("Code Error");
            return;
        }
        var playerInfo = pt.GetPlayerInfo(code);
        var player = GamePlayerInfo.instance.GetOfficialPlayer(index);
        player.index = index;
        foreach (var item in player.training)
        {
            var ti = pt.GetTrainingInfo(item);
            ti.AddStats(playerInfo);
        }
        Sprite illustration = pt.GetPlayerSprite(code);
        Sprite skillIcon = pt.GetSkillInfo(playerInfo.UniqueSkillCode).icon;
        var skillName = gameManager.str.Get(pt.GetSkillInfo(playerInfo.UniqueSkillCode).name.ToString());
        var name = GamePlayerInfo.instance.GetOfficialPlayer(index).name;
        int playerHp = (int)pt.CalculateCurrStats(playerInfo.hp, player.level);
        int playerAttack = (int)pt.CalculateCurrStats(playerInfo.atk, player.level);
        //var grade = pt.starsSprites[GamePlayerInfo.instance.GetOfficialPlayer(index - 1).grade - 3];
        var grade = GamePlayerInfo.instance.GetOfficialPlayer(index).grade - 3;

        var type = pt.playerTypeSprites[GamePlayerInfo.instance.GetOfficialPlayer(index).type - 1];
        var level = GamePlayerInfo.instance.GetOfficialPlayer(index).level;
        var condition = GamePlayerInfo.instance.GetOfficialPlayer(index).condition;
        //var skillIcon;
        //var skillName;
        var skillLevel = GamePlayerInfo.instance.GetOfficialPlayer(index).skillLevel;

        // 이 인덱스 생각하기
        CreateEntryPlayer(parent, index, illustration, name, playerHp, playerAttack, grade, type, level, conditionIcon[condition], skillLevel, skillIcon, skillName);
    }

    public void SetOriginMemberIndex()
    {
        for (int entryInedx = 0; entryInedx < 5; ++entryInedx)
        {
            GameInfo.instance.entryMembersIndex.Add(entryInedx);
        }
        for (int benchIndex = 5; benchIndex < 8; ++benchIndex)
        {
            GameInfo.instance.benchMembersIndex.Add(benchIndex);
        }
    }
    public void SetPlayerEntrySlotAndBenchSlot()
    {
        foreach (int entryIndex in GameInfo.instance.entryMembersIndex)
        {
            SetEntryPlayerSlot(entryScrollView, entryIndex);
        }
        foreach (int benchIndex in GameInfo.instance.benchMembersIndex)
        {
            SetEntryPlayerSlot(benchScrollView, benchIndex);
        }
    }

    // 배틀레이아웃 포지로 넘어가는 함수
    public void SetBattleLayoutForge()
    {
        GameInfo.instance.StartGame();

        // 엔트리 결정 후 라인 지정 가기전에 해줘야 할 것들
        gameManager.entryPanel.SetActiveEntryPanel(false);

        if (gameManager.gameRuleManager.gameType == GameType.Official)
        {
            GameInfo.instance.SetEntryMemeberIndex(entryMembers);
            GameInfo.instance.SetBenchMemberIndex(benchMembers);
            GameInfo.instance.SetEntryPlayer(GameInfo.instance.entryMembersIndex);
        }
        GameInfo.instance.MakePlayers();
        gameManager.settingAIID.SetAIIDs();
        //gameManager.entryManager.RefreshSelectLineButton();

        // 라인 지정 UI
        if (gameManager.gameRuleManager.gameType == GameType.Official)
            gameManager.entryManager.ResetBattleLayoutForge(GameInfo.instance.entryMembersIndex);
        // 라인 지정 UI On
        gameManager.battleLayoutForge.SetActiveBattleLayoutForge(true);
    }
}
