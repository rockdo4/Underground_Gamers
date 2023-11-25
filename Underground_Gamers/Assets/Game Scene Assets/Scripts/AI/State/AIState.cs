using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : BaseState
{
    protected AIController aiController;
    protected CharacterStatus aiStatus;
    protected Transform aiTr;
    protected NavMeshAgent agent;
    protected Animator animator;

    protected float lastDetectTime = 0f;

    public float DistanceToTarget
    {
        get
        {
            if (aiController.target == null)
            {
                return 0f;
            }
            return Vector3.Distance(aiController.transform.position, aiController.target.transform.position);
        }
    }

    public AIState(AIController aiController)
    {
        this.aiController = aiController.GetComponent<AIController>();
        aiStatus = aiController.GetComponent<CharacterStatus>();
        agent = aiController.GetComponent<NavMeshAgent>();
        animator = aiController.GetComponent<Animator>();
        aiTr = aiController.GetComponent<Transform>();
    }

    public void SetTarget(Transform target)
    {
        aiController.target = target;
        agent.SetDestination(aiController.target.position);
        //Debug.Log(this.target);
    }
    protected void RotateToTarget()
    {
        if (aiController.target == null || aiTr == null)
            return;
        Quaternion targetRotation = Quaternion.LookRotation(aiController.target.position - aiTr.position);
        aiTr.rotation = Quaternion.RotateTowards(aiTr.rotation, targetRotation, aiStatus.reactionSpeed * Time.deltaTime);
    }

    // ºÎÃ¤²Ã Å½»ö
    protected void SearchTargetInSector()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.sight, aiController.enemyLayer);
        if (enemyCols.Length == 0 && aiController.target == null)
        {
            //Debug.Log("Not Founded");
            SetTarget(aiController.point);
            aiController.SetState(States.MissionExecution);
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
            float dotAngle = 1f - (aiStatus.sightAngle / 2) * 0.01f;

            if (dot < dotAngle)
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
            aiController.SetState(States.Trace);
            return;
        }
    }

    // Å½Áö¹üÀ§ Å½»ö
    protected void SearchTargetInDetectionRange()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.detectionRange, aiController.enemyLayer);
        if (enemyCols.Length == 0 && aiController.target == null)
        {
            //Debug.Log("Not Founded");
            SetTarget(aiController.point);
            aiController.SetState(States.MissionExecution);
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
            aiController.SetState(States.Trace);
            return;
        }

    }
}
