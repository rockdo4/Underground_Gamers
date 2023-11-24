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
        aiController.RefreshDebugAIStatus(this.ToString());

        lastDetectTime -= aiController.detectTime;
        agent.isStopped = false;
        agent.angularSpeed = aiStatus.reactionSpeed;
        agent.speed = aiStatus.speed;

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        //Debug.Log("Trace State");
        if (!aiStatus.IsLive)
        {
            return;
        }

        if (aiController.target == null)
        {
            aiController.SetState(States.Idle);
            return;
        }

        //if (aiStatus.range > DistanceToTarget)
        //{
        //    aiController.SetState(States.Attack);
        //    return;
        //}

        if (lastDetectTime + aiController.detectTime < Time.time)
        {
            lastDetectTime = Time.time;

            SearchTargetInDetectionRange();
            SearchTargetInSector();

            agent.SetDestination(aiController.target.position);
        }
    }

    // ºÎÃ¤²Ã Å½»ö
    private void SearchTargetInSector()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.sight, aiController.enemyLayer);
        if (enemyCols.Length == 0 && aiController.target == null)
        {
            //Debug.Log("Not Founded");
            SetTarget(aiController.point);
            return;
        }
        Transform target = null;
        var targetToDis = float.MaxValue;
        foreach (var col in enemyCols)
        {
            var dirToTarget = col.transform.position - aiTr.position;
            dirToTarget.Normalize();
            if (Physics.Raycast(aiTr.position, dirToTarget, aiStatus.sight, aiController.obstacleLayer))
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
        {
            SetTarget(target);
            aiController.SetState(States.AimSearch);
            return;
        }
    }

    // Å½Áö¹üÀ§ Å½»ö
    private void SearchTargetInDetectionRange()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.detectionRange, aiController.enemyLayer);
        if (enemyCols.Length == 0 && aiController.target == null)
        {
            //Debug.Log("Not Founded");
            SetTarget(aiController.point);
            return;
        }

        Transform target = null;
        var targetToDis = float.MaxValue;

        foreach (var col in enemyCols)
        {
            var dirToTarget = col.transform.position - aiTr.position;
            dirToTarget.Normalize();
            if (Physics.Raycast(aiTr.position, dirToTarget, aiStatus.sight, aiController.obstacleLayer))
                continue;

            var dis = Vector3.Distance(col.transform.position, aiTr.position);
            if (targetToDis > dis)
            {
                targetToDis = dis;
                target = col.transform;
            }
        }
        if (target != null)
        {
            SetTarget(target);
            aiController.SetState(States.AimSearch);
            return;
        }

    }
}