using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvoidKiting.Asset", menuName = "KitingData/AvoidKiting")]
public class AvoidKiting : KitingData
{
    public GameObject point;
    public override void UpdateKiting(Transform target, AIController ctrl)
    {
        Vector3 enemyLook = ctrl.transform.position - target.position;
        enemyLook.Normalize();

        // 랜덤한 각도를 생성합니다.
        float randomAngle = Random.Range(-45f, 45f); // -45도에서 45도 사이의 각도
        Quaternion randomRotation = Quaternion.Euler(0f, randomAngle, 0f);

        // 현재 방향을 랜덤하게 회전합니다.
        Vector3 randomDirection = randomRotation * enemyLook;

        float distanceToTarget = Vector3.Distance(ctrl.transform.position, target.transform.position);

        Vector3 kitingPos = (randomDirection * (ctrl.status.range - distanceToTarget)) + ctrl.transform.position;
        ctrl.SetDestination(kitingPos);
        ctrl.kitingPos = kitingPos;

        //Instantiate(point, kitingPos, Quaternion.identity);
    }
}
