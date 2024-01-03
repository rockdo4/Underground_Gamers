using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillModeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float switchModeTime = 1f;
    private float switchModeTimer;
    public bool isHover = false;
    public bool IsAutoMode { get; private set; } = true;

    public GameManager gameManager;

    public GameObject rotateAutoIcon;

    public Image coolTimeFill;
    public TextMeshProUGUI coolTimeText;
    public TextMeshProUGUI autoText;
    public Image skillIcon;
    public Image priorSkillImage;

    private AIController currentAI;

    // Update is called once per frame
    void Update()
    {
        if (isHover)
        {
            if (switchModeTimer + switchModeTime < Time.time)
            {
                isHover = false;
                SwitchSkillMode();
            }
        }
    }

    private void SwitchSkillMode()
    {
        IsAutoMode = !IsAutoMode;
        if (IsAutoMode)
        {
            foreach (var ai in gameManager.aiManager.pc)
            {
                ai.isPrior = false;
            }
            priorSkillImage.gameObject.SetActive(false);
        }
        SwitchActiveObject(IsAutoMode);
    }

    private void SwitchActiveObject(bool isActive)
    {
        if (isActive)
        {
            Debug.Log("Auto");
        }
        else
        {
            Debug.Log("Manual");
        }
        // 수동
        coolTimeText.gameObject.SetActive(!isActive);
        // 오토
        rotateAutoIcon.SetActive(isActive);
        autoText.gameObject.SetActive(isActive);
    }

    public void UseOriginSkill()
    {

    }

    public void RefreshUsedSkillCoolTime()
    {
        gameManager.skillCoolTimeManager.CheckCurrentAISkill();
    }

    public void DisplayCoolTimeFillImage(float time)
    {
        coolTimeFill.fillAmount = 1f - time;
    }

    public void DisplayCoolTimeText(float time)
    {
        coolTimeText.text = $"{Mathf.RoundToInt(time)}";
    }

    public void SetActiveCoolTimeFillImage(bool isActive)
    {
        coolTimeFill.gameObject.SetActive(isActive);
    }

    public void SetActiveCoolTimeText(bool isActive)
    {
        coolTimeText.gameObject.SetActive(isActive);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHover = true;
        switchModeTimer = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHover = false;
        if (!IsAutoMode && switchModeTimer + switchModeTime > Time.time)
        {
            // 스킬 수동 사용
            Debug.Log(currentAI.name);
            Debug.Log("Use Skill");

            if (currentAI != null)
            {
                currentAI.isPrior = !currentAI.isPrior;
                SetPriorSkill(currentAI.isPrior);
            }
        }
    }

    public void SetPriorSkill(bool isActive)
    {
        priorSkillImage.gameObject.SetActive(isActive);
    }

    public void SetAI(AIController ai)
    {
        currentAI = ai;
        if (currentAI != null)
            skillIcon.sprite = currentAI.skillIcon;
    }

    public AIController GetAI()
    {
        return currentAI;
    }

    public void SetActiveSkillModeButton(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
