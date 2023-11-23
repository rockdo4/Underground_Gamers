using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceState : AIState
{
    private float lastDetectTime = 0f;
    public TraceState(AIController aiController) : base(aiController)
    {

    }

    public override void Enter()
    {
        lastDetectTime = Time.time - aiController.detectTime;
        agent.isStopped = false;
        agent.speed = aiController.status.speed;
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (target == null)
        {
            aiController.SetState(States.Idle);
            return;
        }

        RotateToTarget();

        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            SearchTargetInDetectionRange();
            SearchTargetInSector();
            agent.SetDestination(target.position);
        }
    }

    private void RotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - aiTr.position);
        aiTr.rotation = Quaternion.RotateTowards(aiTr.rotation, targetRotation, aiStatus.reactionSpeed * Time.deltaTime);
    }

    // ºÎÃ¤²Ã Å½»ö
    private void SearchTargetInSector()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.sight, enemyLayer);
        if (enemyCols.Length == 0)
        {
            Debug.Log("Not Founded");
            SetTarget(aiController.point);
            return;
        }
        Transform target = null;
        var targetToDis = float.MaxValue;
        foreach (var col in enemyCols)
        {
            var dirToTarget = col.transform.position - aiTr.position;
            dirToTarget.Normalize();
            if (Physics.Raycast(aiTr.position, dirToTarget, aiStatus.sight, obstacleLayer))
                continue;

            float dot = Vector3.Dot(dirToTarget, aiTr.forward);

            if (dot / 2 > aiStatus.sightAngle)
                return;

            var dis = Vector3.Distance(col.transform.position, aiTr.position);
            if (targetToDis > dis)
            {
                targetToDis = dis;
                target = col.transform;
            }
        }
        if (target != null)
            SetTarget(target);
    }

    // Å½Áö¹üÀ§ Å½»ö
    private void SearchTargetInDetectionRange()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.detectionRange, enemyLayer);
        if (enemyCols.Length == 0)
        {
            Debug.Log("Not Founded");
            SetTarget(aiController.point);
            return;
        }

        Transform target = null;
        var targetToDis = float.MaxValue;

        foreach (var col in enemyCols)
        {
            var dirToTarget = col.transform.position - aiTr.position;
            dirToTarget.Normalize();
            if (Physics.Raycast(aiTr.position, dirToTarget, aiStatus.sight, obstacleLayer))
                continue;

            var dis = Vector3.Distance(col.transform.position, aiTr.position);
            if (targetToDis > dis)
            {
                targetToDis = dis;
                target = col.transform;
            }
        }
        if (target != null)
            SetTarget(target);
    }
}