using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetPriority.Asset", menuName = "TargetPriority/TargetPriority")]
public class TargetPriority : ScriptableObject
{
    // 탐색에 사용, 반격에 사용, 시야 공유는 사용 X
    public virtual bool SetTargetByPriority(AIController ai, CharacterStatus target)
    {
        float targetDistance = Vector3.Distance(ai.transform.position, target.transform.position);
        if (ai.DistanceToBattleTarget > targetDistance)
        {
            //ai.target = targetIdentity.transform;
            return true;
        }
        return false;
    }
}
