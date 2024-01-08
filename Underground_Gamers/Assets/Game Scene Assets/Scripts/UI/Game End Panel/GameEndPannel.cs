using DG.Tweening;
using EPOOutline;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPannel : MonoBehaviour
{
    public GameManager gameManager;

    //public TextMeshProUGUI winText;
    //public TextMeshProUGUI LoseText;
    public Image victoryLogo;
    public Image defeatLogo;
    public Image youWinLogo;
    public Image youLoseLogo;
    public Vector3 originPos = Vector3.zero;
    public GameObject endPanel;

    public Vector3 targetPos = Vector3.zero;
    public float moveDuration = 2f;

    public TextMeshProUGUI retryStageButtonText;
    public TextMeshProUGUI nextStageButtonText;
    public TextMeshProUGUI damageGraphButtonText;
    public TextMeshProUGUI backButtonText;
    public TextMeshProUGUI nextRoundButtonText;
    public TextMeshProUGUI endRoundButtonText;
    public TextMeshProUGUI okGraphButtonText;

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

    private void Start()
    {
        //winText.text = gameManager.str.Get("win");
        //LoseText.text = gameManager.str.Get("lose");
        retryStageButtonText.text = gameManager.str.Get("retry stage");
        nextStageButtonText.text = gameManager.str.Get("next stage");
        damageGraphButtonText.text = gameManager.str.Get("damage graph");
        backButtonText.text = gameManager.str.Get("back");
        nextRoundButtonText.text = gameManager.str.Get("next round");
        endRoundButtonText.text = gameManager.str.Get("end round");
        okGraphButtonText.text = gameManager.str.Get("ok");
    }

    public void DisplayResultLogo(bool isWin)
    {
        endPanel.SetActive(true);
        if (isWin)
        {
            youWinLogo.gameObject.SetActive(true);
            youLoseLogo.gameObject.SetActive(false);
            originPos = youWinLogo.transform.position;
            youWinLogo.GetComponent<RectTransform>().DOAnchorPos(targetPos, moveDuration).SetEase(Ease.OutQuint);
        }
        else
        {
            youLoseLogo.gameObject.SetActive(true);
            youWinLogo.gameObject.SetActive(false);
            originPos = youLoseLogo.transform.position;
            youLoseLogo.GetComponent<RectTransform>().DOAnchorPos(targetPos, moveDuration).SetEase(Ease.OutQuint);
        }
    }

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
            gameManager.aiRewardManager.DisplayGetXp(GameInfo.instance.XpRewards);
            if (GameInfo.instance.gameType == GameType.Story)
            {
                gameManager.aiRewardManager.DisplayFillXpGauage();
            }

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

        if(GameInfo.instance.gameType != GameType.Official)
        {
            for(int i = 5; i< 8; ++i)
            {
                if (GameInfo.instance.entryPlayer[i].code < 0)
                    continue;
                AIReward aiReward = Instantiate(rewardPrefab, rewardParent);
                aiReward.player = GameInfo.instance.entryPlayer[i];
                aiReward.aiNameText.text = $"{GameInfo.instance.entryPlayer[i].name}";
                aiReward.lvText.text = $"Lv. {GameInfo.instance.entryPlayer[i].level}";
                aiReward.illustration.sprite = gameManager.pt.GetPlayerSprite(GameInfo.instance.entryPlayer[i].code);
                aiReward.aiClass.sprite = gameManager.pt.playerTypeSprites[GameInfo.instance.entryPlayer[i].type - 1];
                aiReward.grade.sprite = gameManager.pt.starsSprites[GameInfo.instance.entryPlayer[i].grade - 3];
                aiReward.currentXP = GameInfo.instance.entryPlayer[i].xp;
                aiReward.maxXP = GameInfo.instance.entryPlayer[i].maxXp;
                gameManager.aiRewardManager.rewards.Add(aiReward);
            }
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
        GameObject[] effects = GameObject.FindGameObjectsWithTag("Effect");
        foreach (var effect in effects)
        {
            Destroy(effect.gameObject);
        }

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
        GameInfo.instance.SetOfficialPlayerCondition();
        gameManager.entryPanel.SetConditionIcon();
        GameObject[] effects = GameObject.FindGameObjectsWithTag("Effect");
        foreach(var effect in effects)
        {
            Destroy(effect.gameObject);
        }
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
        GameObject[] effects = GameObject.FindGameObjectsWithTag("Effect");
        foreach (var effect in effects)
        {
            Destroy(effect.gameObject);
        }

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
        GameObject[] effects = GameObject.FindGameObjectsWithTag("Effect");
        foreach (var effect in effects)
        {
            Destroy(effect.gameObject);
        }

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
        GameObject[] effects = GameObject.FindGameObjectsWithTag("Effect");
        foreach (var effect in effects)
        {
            Destroy(effect.gameObject);
        }

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
        if (GameInfo.instance.isFirstClear)
        {
            GamePlayerInfo.instance.willOpenMenu = (int)GameInfo.instance.gameType + 1;
            uiCanvas.SetActive(false);
            gameManager.killLogPanel.ClearKillLog();
            OffOutline();
            sceneLoader.SceneLoad("Lobby Scene");
            return;
        }
        else if (GameInfo.instance.currentStage != 406)
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
        foreach (var outline in outlinables)
        {
            outline.OutlineParameters.Color = color;
        }
    }
}
