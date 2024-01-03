using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntryPlayer : MonoBehaviour, IPointerClickHandler
{
    public int Index { get; private set; }

    public TextMeshProUGUI playerNameText;
    public Image illustrationIcon;
    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI playerAttackText;
    public Image gradeIcon;
    public Image classIcon;
    public TextMeshProUGUI levelText;
    public Image conditionIcon;
    public TextMeshProUGUI skillLevelText;
    public Image skillIcon;
    public TextMeshProUGUI skillNameText;

    public GameObject selectOutline;
    public Image bg;

    public bool isEntry = false;
    public bool isSelected = false;

    private GameManager gameManager;
    public Color[] outlineColors = new Color[3];

    public void SetInfo(GameManager gameManager, int index, Sprite illustration, string name, int playerHp,
        int playerAttack, int grade, Sprite type, int level, Sprite codition, int skillLevel, Sprite skillIcon, string skillName)
    {
        this.gameManager = gameManager;
        this.Index = index;
        playerNameText.text = $"{name}";
        illustrationIcon.sprite = illustration;
        playerHpText.text = $"{this.gameManager.str.Get("hp")} {playerHp}";
        playerAttackText.text = $"{this.gameManager.str.Get("atk")} {playerAttack}";
        gradeIcon.sprite = this.gameManager.pt.starsSprites[grade];
        SetBgColor(grade);
        classIcon.sprite = type;
        levelText.text = $"{this.gameManager.str.Get("lv")} {level}";
        conditionIcon.sprite = codition;
        skillLevelText.text = $"{this.gameManager.str.Get("skill lv")} {skillLevel}";
        this.skillIcon.sprite = skillIcon;
        skillNameText.text = skillName;
    }

    public void SetConditionIcon(Sprite conditionIcon)
    {
        this.conditionIcon.sprite = conditionIcon;
        //conditionIcon.sprite = GamePlayerInfo.instance.GetOfficialPlayer(Index).condition;
    }

    public void SetBgColor(int grade)
    {
        bg.color = outlineColors[grade];
    }
    public void SetActiveSelectOutline(bool isActive)
    {
        selectOutline.SetActive(isActive);
    }
    public void ClickEntryPlayer()
    {
        if (isEntry)
        {
            ClickEntryMember();
        }
        else
            ClickBenchMember();

        gameManager.entryPanel.SwapEntryMember();
    }

    public void ClickEntryMember()
    {
        if (gameManager.entryPanel.selectedEntryMember == this) // 같은 멤버를 다시 클릭한 경우
        {
            gameManager.entryPanel.selectedEntryMember.SetActiveSelectOutline(false); // 선택 해제
            gameManager.entryPanel.selectedEntryMember = null;
        }
        else // 다른 멤버를 클릭한 경우
        {
            if (gameManager.entryPanel.selectedEntryMember != null)
            {
                gameManager.entryPanel.selectedEntryMember.SetActiveSelectOutline(false); // 이전 멤버 선택 해제
            }

            gameManager.entryPanel.selectedEntryMember = this; // 새로운 멤버 선택
            gameManager.entryPanel.selectedEntryMember.SetActiveSelectOutline(true); // 선택한 멤버 활성화
        }
    }

    public void ClickBenchMember()
    {
        if (gameManager.entryPanel.selectedBenchMember == this) // 같은 멤버를 다시 클릭한 경우
        {
            gameManager.entryPanel.selectedBenchMember.SetActiveSelectOutline(false); // 선택 해제
            gameManager.entryPanel.selectedBenchMember = null;
        }
        else // 다른 멤버를 클릭한 경우
        {
            if (gameManager.entryPanel.selectedBenchMember != null)
            {
                gameManager.entryPanel.selectedBenchMember.SetActiveSelectOutline(false); // 이전 멤버 선택 해제
            }

            gameManager.entryPanel.selectedBenchMember = this; // 새로운 멤버 선택
            gameManager.entryPanel.selectedBenchMember.SetActiveSelectOutline(true); // 선택한 멤버 활성화
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickEntryPlayer();
    }
}
