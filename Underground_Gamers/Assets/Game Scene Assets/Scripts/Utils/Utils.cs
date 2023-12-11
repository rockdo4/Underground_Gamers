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
        Transform wayPoint = null;
        float nearestDistance = float.MaxValue;


        for (int i = 1; i < paths.Length; ++i)
        {

            float distance = Vector3.Distance(ai.transform.position, paths[i].position);
            if (nearestDistance > distance)
            {
                nearestDistance = distance;
                wayPoint = paths[i];
            }
        }

        return wayPoint;
    }
}
