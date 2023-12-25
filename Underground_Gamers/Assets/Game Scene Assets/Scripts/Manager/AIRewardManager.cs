using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRewardManager : MonoBehaviour
{
    public List<AIReward> rewards;

    public void ClearRewards()
    {
        rewards.Clear();
    }

    public void DisplayRewardResult()
    {
        foreach(var reward in rewards)
        {
            reward.CalXp();
            reward.DisplayLevel();
        }
    }

    public void DisplayFillXpGauage()
    {
        foreach(var reward in rewards)
        {
            reward.FillXpGauge(1f);
        }
    }
}
