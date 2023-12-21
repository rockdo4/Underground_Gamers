using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPannel : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI LoseText;

    public GameObject rewardPanel;
    [Header("딜 그래프 패널")]
    public GameObject damageGraphPanel;
    public GameObject nextRoundButton;
    public GameObject okButton;
    public GameObject endRoundButton;

    public Transform rewardParent;
    public Transform damageGraphParent;

    public AIReward rewardPrefab;
    public DamageGraph damageGraphPrefab;

    private float maxDealtDamage = 0;
    private float maxTakenDamage = 0;
    private float maxHealAmount = 0;

    public void ResetDamageGraph()
    {
        maxDealtDamage = 0;
        maxTakenDamage = 0;
        maxHealAmount = 0;
    }

    public void ResetTotalKillCount()
    {
        var npcKillCounttext = GameObject.FindGameObjectWithTag("NPC_Score").GetComponent<TMP_Text>();
        npcKillCounttext.text = $"{0}";
        var pcKillCounttext = GameObject.FindGameObjectWithTag("PC_Score").GetComponent<TMP_Text>();
        pcKillCounttext.text = $"{0}";
    }

    public void OnGameEndPanel()
    {
        gameObject.SetActive(true);
        if (gameManager.gameRuleManager.WinningCount == 1)
        {
            OffDamageGraph();
            OnRewardPanel();
        }
        else
        {
            OnDamageGraph();
            OffRewardPanel();
        }
        CreateAIReward();
        CreateDamageGraph();
    }

    public void CreateAIReward()
    {
        foreach (var pc in gameManager.aiManager.pc)
        {
            AIReward aiReward = Instantiate(rewardPrefab, rewardParent);
            //이름
            aiReward.aiNameText.text = $"{pc.status.AIName}";
            aiReward.lvText.text = $"Lv. {pc.status.lv}";
            aiReward.illustration.sprite = pc.status.illustration;
            aiReward.aiClass.sprite = pc.status.aiClass;
            aiReward.grade.sprite = pc.status.grade;
        }
    }

    public void CreateDamageGraph()
    {
        foreach (var pc in gameManager.aiManager.pc)
        {
            DamageGraph damageGraph = Instantiate(damageGraphPrefab, damageGraphParent);
            // 여기서 이미지 받기
            pc.damageGraph = damageGraph;
            damageGraph.illustration.sprite = pc.status.illustration;
            damageGraph.dealtDamage = pc.status.dealtDamage;
            damageGraph.takenDamage = pc.status.takenDamage;
            damageGraph.healAmount = pc.status.healAmount;

            if (maxDealtDamage < damageGraph.dealtDamage)
            {
                maxDealtDamage = damageGraph.dealtDamage;
            }

            if (maxTakenDamage < damageGraph.takenDamage)
            {
                maxTakenDamage = damageGraph.takenDamage;
            }

            if (maxHealAmount < damageGraph.healAmount)
            {
                maxHealAmount = damageGraph.healAmount;
            }
        }

        foreach (var pc in gameManager.aiManager.pc)
        {
            pc.damageGraph.SetMaxDamages(maxDealtDamage, maxTakenDamage, maxHealAmount);
            pc.damageGraph.DisplayDamges();
        }
    }

    public void EnterLobby()
    {
        SceneManager.LoadScene("Lobby Scene");
    }

    public void EnterNextRound()
    {
        // 빌딩 재건축 및, BuildingManger등록 
        // GameRuleManager에 빌딩 등록

        // AIManager에서 제거 
        // SkillCoolTimeManager 비우기 시간 초기화, UI 신경쓰기
        // 엔트리 지정
        // 기존 명령 패널 있는거 삭제 
        // 캐릭터 제거 ResetAI

        // 게임 시간 리셋
        // 킬로그 패널 지우기
        // 이펙트 다 지우기
        // 라인 매니저 생각하기, 안해도 될듯
        // 킬스코어 초기화/ 건물 체력 리셋

        gameManager.aiManager.ResetAI();
        gameManager.skillCoolTimeManager.ResetSkillCooldown();
        gameManager.respawner.ClearRespawn();
        gameManager.commandManager.ResetCommandInfo();
        gameManager.buildingManager.ResetBuildings();

        // Time.timeScale =1f 신경쓰기
        gameManager.PlayingGame();
        ResetDamageGraph();
        ResetTotalKillCount();

        // PlayingGame안에서 처리
        //gameManager.gameEndPannel.OffGameEndPanel();
        GameInfo.instance.MakePlayers();
        gameManager.settingAIID.SetAIIDs();
        gameManager.entryManager.ResetEntry();

        gameManager.battleLayoutForge.SetActiveBattleLayoutForge(true);
        gameManager.entryManager.RefreshSelectLineButton();
        gameManager.lineManager.ResetAllLines();
    }

    public void EndRound()
    {
        gameManager.aiManager.ResetAI();
        GamePlayerInfo.instance.CalculateOfficialPlayer(gameManager.IsGameWin,
            gameManager.gameRuleManager.PCWinEvidenceCount,
            gameManager.gameRuleManager.NPCWinEvidenceCount);
        SceneManager.LoadScene("Lobby Scene");
    }

    public void OffGameEndPanel()
    {
        gameObject.SetActive(false);
    }

    public void OnDamageGraph()
    {
        damageGraphPanel.SetActive(true);
        SetActiveDamageOkButton(gameManager.gameRuleManager.WinningCount == 1);
        SetActiveDamageNextRoundButton(!gameManager.IsRoundEnd && gameManager.gameRuleManager.WinningCount != 1);
        SetActiveDamageEndRoundButton(gameManager.IsRoundEnd && gameManager.gameRuleManager.WinningCount != 1);
    }

    public void OnRewardPanel()
    {
        rewardPanel.SetActive(true);
    }
    public void OffDamageGraph()
    {
        damageGraphPanel.SetActive(false);
    }

    public void SetActiveDamageOkButton(bool isActive)
    {
        okButton.SetActive(isActive);
    }
    public void SetActiveDamageNextRoundButton(bool isActive)
    {
        nextRoundButton.SetActive(isActive);
    }
    public void SetActiveDamageEndRoundButton(bool isActive)
    {
        endRoundButton.SetActive(isActive);
    }

    public void OffRewardPanel()
    {
        rewardPanel.SetActive(false);
    }
}
