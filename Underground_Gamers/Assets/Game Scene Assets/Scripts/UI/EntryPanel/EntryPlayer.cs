using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntryPlayer : MonoBehaviour, IPointerClickHandler
{
    int index;

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
    private bool isSelected = false;

    private GameManager gameManager;

    public void SetInfo(GameManager gameManager, int index, Sprite illustration, string name, int playerHp, int playerAttack, Sprite grade, Sprite type, int level, Sprite codition, int skillLevel)
    {
        this.gameManager = gameManager;
        this.index = index;
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

        if (isSelected && isEntry)
        {
            gameManager.entryPanel.SetActiveEntryMembers(false);
            gameManager.entryPanel.selectedEntryInedx = index;
            Debug.Log(gameManager.entryPanel.selectedEntryInedx);
        }

        if (isSelected && !isEntry)
        {
            gameManager.entryPanel.SetActiveBenchMembers(false);
            gameManager.entryPanel.selectedBenchIndex = index;
            Debug.Log(gameManager.entryPanel.selectedBenchIndex);
        }
        isSelected = !isSelected;
        SetActiveSelectOutline(isSelected);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickEntryPlayer();
    }
}
