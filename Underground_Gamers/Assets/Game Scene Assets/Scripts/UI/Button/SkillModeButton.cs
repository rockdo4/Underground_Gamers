using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillModeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float switchModeTime = 1f;
    private float switchModeTimer;
    public bool isHover = false;
    private bool isAutoMode = true;

    public GameObject rotateAutoIcon;



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
        SwitchAutoIcon(isAutoMode);
    }

    private void SwitchAutoIcon(bool isActive)
    {
        if(isActive)
        {
            Debug.Log("Auto");
        }
        else
        {
            Debug.Log("Manual");
        }
        rotateAutoIcon.SetActive(isActive);
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
            Debug.Log("Use Skill");
        }
    }
}
