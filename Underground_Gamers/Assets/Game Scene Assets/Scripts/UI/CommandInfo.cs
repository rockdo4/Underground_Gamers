using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Condition
{
    Smile,
    Neutral,
    Sad,
    Count
}

public class CommandInfo : MonoBehaviour
{
    public string aiType;
    public int aiNum;

    private GameManager gameManager;

    public AIController aiController;
    public GameObject respawnTimeUI;
    public TextMeshProUGUI respawnTimeText;
    public Image respawnCoolTime;

    public Transform respawner;

    public GameObject aiSelectImage;
    public Image portrait;

    public TextMeshProUGUI aiName;

    public GameObject statusInfo;

    public Slider hpBar;
    public TextMeshProUGUI killCountText;
    public Image conditionIcon;
    public Sprite[] conditions = new Sprite[(int)Condition.Count];

    public Image skillCoolTime;
    public Image internalClassIcon;
    public Image externalClassIcon;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        respawnTimeUI.SetActive(false);
        aiSelectImage.SetActive(false);
    }

    public void OnRespawnUI(float time)
    {
        respawnTimeText.text = $"{Mathf.RoundToInt(time)}";
        respawnTimeUI.SetActive(true);
    }

    public void OffRespawnUI()
    {
        respawnTimeUI.SetActive(false);
    }

    public void DisplayRespawnTime(float time)
    {
        respawnTimeText.text = $"{Mathf.RoundToInt(time)}";
    }

    public void RefreshRespawnCoolTime(float coolTime)
    {
        respawnCoolTime.fillAmount = coolTime;
    }

    public void DisplayKillCount(float killCount, float deathCount)
    {
        killCountText.text = $"{killCount} / {deathCount}";
    }

    public void ResetKillCount()
    {
        aiController.status.killCount = 0;
        aiController.status.deathCount = 0;
        killCountText.text = $"{aiController.status.killCount} / {aiController.status.deathCount}";
    }

    public void DisplayHpBar(float value)
    {
        hpBar.value = value;
    }

    public void DisplaySkillCoolTimeFillImage(float value)
    {
        skillCoolTime.fillAmount = value;
    }

    public void SelectAI()
    {
        gameManager.commandManager.SetActiveCommandButton(aiController);
        aiSelectImage.SetActive(true);
    }

    public void UnSelectAI()
    {
        aiSelectImage.SetActive(false);
    }

    public void SetPortraitInCommandInfo(int code)
    {
        portrait.sprite = DataTableManager.instance.Get<PlayerTable>(DataType.Player).GetPlayerSprite(code);
    }

    public void SetClassIcon(Sprite sprite)
    {
        internalClassIcon.sprite = sprite;
        externalClassIcon.sprite = sprite;
    }
}