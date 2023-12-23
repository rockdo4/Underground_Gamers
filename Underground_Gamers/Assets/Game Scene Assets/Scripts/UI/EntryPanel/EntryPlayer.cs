using DG.Tweening.Core.Easing;
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

    public GameObject selectOutline;

    public bool isEntry = false;
    public bool isSelected = false;

    private GameManager gameManager;

    public void SetInfo(GameManager gameManager, int index, Sprite illustration, string name, int playerHp, int playerAttack, Sprite grade, Sprite type, int level, Sprite codition, int skillLevel)
    {
        this.gameManager = gameManager;
        this.Index = index;
        playerNameText.text = $"{name}";
        illustrationIcon.sprite = illustration;
        playerHpText.text = $"{playerHp}";
        playerAttackText.text = $"{playerAttack}";
        gradeIcon.sprite = grade;
        classIcon.sprite = type;
        levelText.text = $"{level}";
        conditionIcon.sprite = codition;
        skillLevelText.text = $"{skillLevel}";
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
