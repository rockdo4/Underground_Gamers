using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "TrackingKiting.Asset", menuName = "KitingData/TrackingKiting")]
public class TrackingKiting : KitingData
{
    public GameObject point;

    [Tooltip("근접 카이팅 범위")]
    public float trackingKitingRange;
    public override void UpdateKiting(Transform target, AIController ctrl)
    {
        int attempt = 0;
        Vector3 kitingRandomPoint = Vector3.zero;

        while (attempt < 30)
        {
            kitingRandomPoint = Utils.RandomPointInCircle(trackingKitingRange, ctrl.target);
            kitingRandomPoint.y = ctrl.transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(kitingRandomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                kitingRandomPoint = hit.position;
                ctrl.SetDestination(kitingRandomPoint);
                ctrl.kitingPos = kitingRandomPoint;
                Instantiate(point, kitingRandomPoint, Quaternion.identity);
                return;
            }

            attempt++;
        }

        ctrl.SetState(States.Idle);
    }
}
