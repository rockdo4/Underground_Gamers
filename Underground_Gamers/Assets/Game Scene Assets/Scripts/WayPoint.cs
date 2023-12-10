using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Line
{
    Top,
    Bottom,
    Count
}

public class WayPoint : MonoBehaviour
{
    public GameObject topZone;
    public GameObject bottomZone;
    [Tooltip("0번째 인덱스 제외")]

    public Transform[] bottomWayPoints;
    public Transform[] topWayPoints;

    private void Awake()
    {
        topWayPoints = topZone.GetComponentsInChildren<Transform>();
        bottomWayPoints = bottomZone.GetComponentsInChildren<Transform>();
    }
}
