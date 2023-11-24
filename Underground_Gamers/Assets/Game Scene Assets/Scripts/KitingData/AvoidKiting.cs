using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidKiting.Asset", menuName = "KitingData/AvoidKiting")]
public class AvoidKiting : KitingData
{
    public override void UpdateKiting(Transform target, AIController ctrl)
    {
        Vector3 enemyLook = ctrl.transform.position - target.position;
        enemyLook.Normalize();

        Vector3 kitingPos = enemyLook * ctrl.status.range;
        ctrl.SetDestination(kitingPos);
    }
}
