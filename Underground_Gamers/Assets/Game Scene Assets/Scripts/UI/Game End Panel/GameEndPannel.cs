using DG.Tweening;
using DG.Tweening.Core.Easing;
using EPOOutline;
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

    public GameObject nextButton;
    public GameObject damageGraphButton;

    public GameObject nextStageButton;
    public GameObject retryButton;

    public List<Outlinable> outlinables = new List<Outlinable>();
    public SceneLoader sceneLoader;
    public GameObject uiCanvas;

    public void SetActiveGameEndPanelButton(bool isActive)
    {
        nextButton.SetActive(isActive);
        damageGraphButton.SetActive(isActive);
    }

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
            SetActiveGameEndPanelButton(true);
        }
        else
        {
            OnDamageGraph();
            OffRewardPanel();
            SetActiveGameEndPanelButton(false);
        }
        CreateAIReward();
        CreateDamageGraph();

        // 집계 하기전
        gameManager.aiRewardManager.DisplayRewardResult();

        // 집계 중
        if (gameManager.IsRoundWin)
        {
            GameInfo.instance.WinReward();
            DOTween.timeScale = 1f;
            DOTween.defaultTimeScaleIndependent = true;
            gameManager.aiRewardManager.DisplayFillXpGauage();
            gameManager.aiRewardManager.DisplayGetXp(GameInfo.instance.XpRewards);

            if (GameInfo.instance.gameType == GameType.Story)
            {
                nextStageButton.SetActive(true);
                retryButton.SetActive(false);
            }
            else
            {
                nextStageButton.SetActive(false);
                retryButton.SetActive(false);
            }
        }
        else
        {
            gameManager.aiRewardManager.DisplayGetXp(0);

            if (GameInfo.instance.gameType == GameType.Story)
            {
                nextStageButton.SetActive(false);
                retryButton.SetActive(true);
            }
            else
            {
                nextStageButton.SetActive(false);
                retryButton.SetActive(false);
            }
        }

        // 집계 후
        // 애니메이션 끝난 후 분기 필요
        //gameManager.aiRewardManager.DisplayRewardResult();
        GamePlayerInfo.instance.ClearLvUpCount();
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
            aiReward.currentXP = pc.status.xp;
            aiReward.maxXP = pc.status.maxXp;
            aiReward.ai = pc;
            gameManager.aiRewardManager.rewards.Add(aiReward);
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
        gameManager.aiManager.ResetAI();
        uiCanvas.SetActive(false);
        GameInfo.instance.ClearEntryPlayer();
        GameInfo.instance.ClearMembersIndex();
        gameManager.aiRewardManager.ClearRewards();
        uiCanvas.SetActive(false);
        gameManager.killLogPanel.ClearKillLog();
        OffOutline();
        sceneLoader.SceneLoad("Lobby Scene");
        //SceneManager.LoadScene("Lobby Scene");
    }

    public void ClearEntry()
    {
        gameManager.commandManager.UnSelect();
        GameInfo.instance.ClearEntryPlayer();
        GameInfo.instance.ClearMembersIndex();
    }

    public void EnterNextRound()
    {
        gameManager.aiManager.UpdateOfficialResult();
        gameManager.autoSelect.ResetButton();

        gameManager.aiManager.ResetAI();
        gameManager.skillCoolTimeManager.ResetSkillCooldown();
        gameManager.respawner.ClearRespawn();
        gameManager.commandManager.ResetCommandInfo();
        gameManager.buildingManager.ResetBuildings();

        // Time.timeScale =1f 신경쓰기
        gameManager.PlayingGame();
        ResetDamageGraph();
        ResetTotalKillCount();

        gameManager.entryPanel.SetActiveEntryPanel(true);
        // 이곳에서 entryPanel의 EntryMembers초기화 해주어야함, 변경점
        GameInfo.instance.ClearEntryPlayer();
        GameInfo.instance.ClearMembersIndex();

        // NextRound 시 매번 되어야 함
        gameManager.lineManager.ResetAllLines();
        gameManager.commandManager.UnSelect();
        gameManager.aiRewardManager.ClearRewards();
    }

    public void EndRound()
    {
        gameManager.aiManager.UpdateOfficialResult();

        GameInfo.instance.ClearEntryPlayer();
        GameInfo.instance.ClearMembersIndex();
        gameManager.aiManager.ResetAI();
        uiCanvas.SetActive(false);
        gameManager.aiRewardManager.ClearRewards();
        GamePlayerInfo.instance.CalculateOfficialPlayer(gameManager.IsGameWin,
            gameManager.gameRuleManager.PCWinEvidenceCount,
            gameManager.gameRuleManager.NPCWinEvidenceCount);
        uiCanvas.SetActive(false);
        gameManager.killLogPanel.ClearKillLog();
        OffOutline();
        sceneLoader.SceneLoad("Lobby Scene");
        //SceneManager.LoadScene("Lobby Scene");
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

    public void Retry()
    {
        gameManager.aiManager.ResetAI();
        GameInfo.instance.ClearEntryPlayer();
        GameInfo.instance.ClearMembersIndex();
        gameManager.aiRewardManager.ClearRewards();
        GameInfo.instance.DeletePlayers();
        uiCanvas.SetActive(false);
        gameManager.killLogPanel.ClearKillLog();
        OffOutline();
        sceneLoader.SceneLoad("Game Scene");
        //SceneManager.LoadScene("Game Scene");
    }

    public void NextStage()
    {
        gameManager.aiManager.ResetAI();
        GameInfo.instance.ClearEntryPlayer();
        GameInfo.instance.ClearMembersIndex();
        gameManager.aiRewardManager.ClearRewards();
        GameInfo.instance.currentStage = GameInfo.instance.currentStage switch
        {
            106 => 201,
            206 => 301,
            306 => 401,
            >= 406 => 406,
            _ => GameInfo.instance.currentStage+1
        };

        GameInfo.instance.DeletePlayers();
        if (GameInfo.instance.currentStage != 406)
        {
            uiCanvas.SetActive(false);
            gameManager.killLogPanel.ClearKillLog();
            OffOutline();
            sceneLoader.SceneLoad("Game Scene");
            //SceneManager.LoadScene("Game Scene");
        }
        else
        {
            uiCanvas.SetActive(false);
            gameManager.killLogPanel.ClearKillLog();
            OffOutline();
            sceneLoader.SceneLoad("Lobby Scene");
            //SceneManager.LoadScene("Lobby Scene");
        }    
    }

    private void OffOutline()
    {
        Color color = Color.white;
        color.a = 0;
        foreach(var outline in outlinables)
        {
            outline.OutlineParameters.Color = color;
        }
    }
}
