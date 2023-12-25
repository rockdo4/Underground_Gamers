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

    public void DisplayXpGauge()
    {
        foreach(var reward in rewards)
        {
            reward.CalXp();
        }
    }
}
