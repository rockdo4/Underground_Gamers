using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryPlayer : MonoBehaviour
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

    public void SetActiveSelectOutline(bool isActive)
    {
        selectOutline.SetActive(isActive);
    }

    public void SetInfo(int index, Sprite illustration, string name, int playerHp, int playerAttack, Sprite grade, Sprite type, int level, Sprite codition, int skillLevel)
    {
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
}
