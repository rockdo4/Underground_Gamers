using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utils
{
    public static bool RandomPointInNav(Vector3 center, float range, int attemp, out Vector3 result)
    {
        for (int i = 0; i < attemp; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = center;
        return false;
    }

    public static Transform FindNearestPoint(AIController ai, Transform[] paths)
    {
        Transform target = ai.battleTarget;
        Transform wayPoint = null;
        float nearestDistance = float.MaxValue;

        // 0번째 path는 제외
        for (int i = 1; i < paths.Length; ++i)
        {
            //ai.SetDestination(paths[i].position);
            //if (nearestRemainingDistance > ai.agent.remainingDistance)
            //{
            //    nearestRemainingDistance = ai.agent.remainingDistance;
            //    wayPoint = paths[i];
            //}
            float distance = Vector3.Distance(ai.transform.position, paths[i].position);
            if (nearestDistance > distance)
            {
                nearestDistance = distance;
                wayPoint = paths[i];
            }
        }

        //ai.SetDestination(target.position);
        return wayPoint;
    }
}
