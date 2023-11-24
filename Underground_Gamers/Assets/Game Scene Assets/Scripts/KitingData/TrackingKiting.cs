using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrackingKiting.Asset", menuName = "KitingData/TrackingKiting")]
public class TrackingKiting : KitingData
{
    [Tooltip("근접 카이팅 범위")]
    public float trackingKitingRange;
    public override void UpdateKiting(Transform target, AIController ctrl)
    {
        Vector3 kitingRandomPoint = Utils.RandomPointInCircle(trackingKitingRange, ctrl.target);
        ctrl.SetDestination(kitingRandomPoint);
    }
}
