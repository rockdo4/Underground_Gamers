using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 RandomPointInCircle(float radius, Transform target)
    {
        float randomAngle = Random.Range(0, Mathf.PI * 2f);
        float x = radius * Mathf.Cos(randomAngle);
        float z = radius * Mathf.Sin(randomAngle);

        Vector3 randomPoint = new Vector3(x, 0, z);
        randomPoint += target.transform.position;
        return randomPoint;
    }

    public static Transform FindNearestRemainingDistance(AIController ai, Transform[] paths)
    {
        Transform target = ai.target;
        Transform wayPoint = null;
        float nearestRemainingDistance = float.MaxValue;

        // 0번째 path는 제외
        for(int i = 1; i < paths.Length; ++i)
        {
            ai.agent.SetDestination(paths[i].position);
            if (nearestRemainingDistance > ai.agent.remainingDistance)
            {
                nearestRemainingDistance = ai.agent.remainingDistance;
                wayPoint = paths[i];
            }

        }

        ai.agent.SetDestination(target.position);
        return wayPoint;
    }
}
