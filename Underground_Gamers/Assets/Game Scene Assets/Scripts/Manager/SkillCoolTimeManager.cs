using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCoolTimeManager : MonoBehaviour
{
    private List<(AIController, float)> skillCooldowns = new List<(AIController, float)>();

    private void Update()
    {
        for (int i = skillCooldowns.Count - 1; i >= 0; --i)
        {
            if (skillCooldowns[i].Item2 + skillCooldowns[i].Item1.originalSkillCoolTime < Time.time)
            {
                skillCooldowns[i].Item1.isOnCoolOriginalSkill = true;
                if (skillCooldowns[i].Item1.aiCommandInfo != null)
                    skillCooldowns[i].Item1.aiCommandInfo.DisplaySkillCoolTime(1f);
                skillCooldowns.RemoveAt(i);
            }
            else
            {
                if (skillCooldowns[i].Item1.aiCommandInfo != null)
                    skillCooldowns[i].Item1.aiCommandInfo.DisplaySkillCoolTime((Time.time - skillCooldowns[i].Item2) / skillCooldowns[i].Item1.originalSkillCoolTime);
            }
        }
    }

    public void StartSkillCooldown(AIController controller, float lastSkillTime)
    {
        skillCooldowns.Add((controller, lastSkillTime));
    }
}
