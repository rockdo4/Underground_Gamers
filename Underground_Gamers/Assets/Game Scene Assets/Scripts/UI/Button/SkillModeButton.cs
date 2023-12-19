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
    private bool isAutoMode = true;
    private bool isPrior = false;

    public GameObject rotateAutoIcon;

    public Image coolTimeFill;
    public TextMeshProUGUI coolTimeText;
    public TextMeshProUGUI autoText;
    public Image priorSkillImage;

    private AIController currentAI;

    // Update is called once per frame
    void Update()
    {
        if(isHover)
        {
            if(switchModeTimer + switchModeTime < Time.time)
            {
                isHover = false;
                SwitchSkillMode();
            }
        }
    }

    private void SwitchSkillMode()
    {
        isAutoMode = !isAutoMode;
        if(isAutoMode)
        {
            isPrior = false;
            priorSkillImage.gameObject.SetActive(false);
        }
        SwitchActiveObject(isAutoMode);
    }

    private void SwitchActiveObject(bool isActive)
    {
        if(isActive)
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

    public void DisplayCoolTimeFillImage(float time)
    {
        coolTimeFill.fillAmount = 1f - time;
    }

    public void DisplayCoolTimeText(float time)
    {
        coolTimeText.text = $"{Mathf.RoundToInt(time)}";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHover = true;
        switchModeTimer = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHover = false;
        if (!isAutoMode && switchModeTimer + switchModeTime > Time.time)
        {
            // 스킬 수동 사용
            Debug.Log(currentAI.name);
            Debug.Log("Use Skill");

            isPrior = !isPrior;
            SetPriorSkill(isPrior);
        }
    }

    public void SetPriorSkill(bool isActive)
    {
        priorSkillImage.gameObject.SetActive(isActive);
    }

    public void SetAI(AIController ai)
    {
        currentAI = ai;
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
