using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SkillCoolTimeManager : MonoBehaviour
{
    private List<(AIController, float)> skillCooldowns = new List<(AIController, float)>();
    public GameManager gameManager;
    public SkillModeButton skillModeButton;

    private bool isSkillUsed = false;

    private void Update()
    {
        for (int i = skillCooldowns.Count - 1; i >= 0; --i)
        {
            if (skillCooldowns[i].Item2 + skillCooldowns[i].Item1.originalSkillCoolTime < Time.time)
            {
                skillCooldowns[i].Item1.isOnCoolOriginalSkill = true;
                if (skillCooldowns[i].Item1.aiCommandInfo != null)
                {
                    skillCooldowns[i].Item1.aiCommandInfo.DisplaySkillCoolTimeFillImage(1f);
                    // 쿨타임 표시해주기
                    if (skillModeButton.GetAI() == skillCooldowns[i].Item1)
                    {
                        skillModeButton.DisplayCoolTimeFillImage(0f);
                        skillModeButton.DisplayCoolTimeText(0f);
                        skillModeButton.SetActiveCoolTimeFillImage(false);
                        skillModeButton.SetActiveCoolTimeText(false);
                    }
                }
                skillCooldowns.RemoveAt(i);
            }
            else
            {
                // 쿨타임 표시해주기
                float timeFillImage = (Time.time - skillCooldowns[i].Item2) / skillCooldowns[i].Item1.originalSkillCoolTime;
                float timeText = skillCooldowns[i].Item1.originalSkillCoolTime - (Time.time - skillCooldowns[i].Item2);
                if (skillCooldowns[i].Item1.aiCommandInfo != null)
                    skillCooldowns[i].Item1.aiCommandInfo.DisplaySkillCoolTimeFillImage(timeFillImage);
                if (skillModeButton.GetAI() == skillCooldowns[i].Item1)
                {
                    skillModeButton.DisplayCoolTimeFillImage(timeFillImage);
                    skillModeButton.DisplayCoolTimeText(timeText);
                }
            }
        }
    }

    public void CheckCurrentAISkill()
    {
        isSkillUsed = !skillCooldowns.Any(ai => ai.Item1 == skillModeButton.GetAI());
        if (!isSkillUsed && !skillModeButton.IsAutoMode)
        {
            skillModeButton.DisplayCoolTimeFillImage(1f);
            skillModeButton.DisplayCoolTimeText(0f);
        }
    }

    public void StartSkillCooldown(AIController controller, float lastSkillTime)
    {
        skillCooldowns.Add((controller, lastSkillTime));
    }

    public void ResetSkillCooldown()
    {
        skillModeButton.DisplayCoolTimeFillImage(0f);
        skillModeButton.DisplayCoolTimeText(0f);
        skillModeButton.SetActiveCoolTimeFillImage(false);
        skillModeButton.SetActiveCoolTimeText(false);
        skillCooldowns.Clear();
    }
}
