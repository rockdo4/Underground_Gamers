using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetPriority.Asset", menuName = "TargetPrioriy/TargetPrioriy")]
public class TargetPriority : ScriptableObject
{
    public virtual bool SetTargetByPriority(AIController ai, AIController target)
    {
        float targetDistance = Vector3.Distance(ai.transform.position, target.transform.position);
        if(ai.DistanceToTarget > targetDistance)
        {
            ai.target = target.transform;
            return true;
        }
        return false;
    }
}
