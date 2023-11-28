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
}
