using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRewardManager : MonoBehaviour
{
    public List<AIReward> rewards;

    public float fillXpGaugeTime = 1.5f;

    public void ClearRewards()
    {
        rewards.Clear();
    }

    public void DisplayGetXp(int getXp)
    {
        foreach (var reward in rewards)
        {
            reward.DisplayGetXp(getXp);
        }
    }

    public void DisplayRewardResult()
    {
        foreach(var reward in rewards)
        {
            reward.CalXp();
            reward.DisplayLevel(reward.ai.playerInfo.level);
        }
    }

    public void DisplayFillXpGauage()
    {
        foreach(var reward in rewards)
        {
            // 몇번 렙업하나 분기
            reward.FillXpGauge(reward.ai.playerInfo.levelUpCount, fillXpGaugeTime);
        }
    }
}
