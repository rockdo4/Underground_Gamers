using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Utils
{
    public static float GetEaseOutQuintReversed(float t)
    {
        return 1 - ((t = 1 - t) * t * t * t * t);
    }    
    
    public static float GetEaseOutQuint(float t)
    {
        return 1f + (--t) * t * t * t * t;
    }

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

    public static float GetRandomDamageByAccuracy(float damage, CharacterStatus aStatus)
    {
        float u1 = 1f - Random.Range(0f, 1f);
        float u2 = 1f - Random.Range(0f, 1f);

        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        float randResult = Mathf.Abs((randStdNormal * 50.0f / aStatus.accuracyRate));
        if (randResult >= 3)
            randResult = 3;
        damage *= (1.1f - (randResult * 0.2f));
        return damage;
    }
}
    