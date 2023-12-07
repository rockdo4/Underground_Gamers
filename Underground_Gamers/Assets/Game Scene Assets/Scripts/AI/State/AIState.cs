using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public AIState(AIController aiController)
    {
        this.aiController = aiController.GetComponent<AIController>();
        aiStatus = aiController.GetComponent<CharacterStatus>();
        agent = aiController.GetComponent<NavMeshAgent>();
        animator = aiController.GetComponent<Animator>();
        aiTr = aiController.GetComponent<Transform>();
    }

    protected void RotateToTarget()
    {
        if (aiController.battleTarget == null || aiTr == null)
            return;
        Quaternion targetRotation = Quaternion.LookRotation(aiController.battleTarget.position - aiTr.position);
        aiTr.rotation = Quaternion.RotateTowards(aiTr.rotation, targetRotation, aiStatus.reactionSpeed * Time.deltaTime);
    }

    // ºÎÃ¤²Ã Å½»ö
    protected void SearchTargetInSector()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.sight, aiController.enemyLayer);
        if (enemyCols.Length == 0 && aiController.battleTarget == null)
        {
            //aiController.SetBattleTarget(aiController.point);
            aiController.SetState(States.MissionExecution);
            return;
        }
        Transform target = null;
        //var targetToDis = float.MaxValue;
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
           // Debug.Log($"Col : {col.name}");

            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            for (int i = 0; i < aiController.priorityByOccupation.Count; ++i)
            {
                if (aiController.priorityByOccupation[i].SetTargetByPriority(aiController, colStatus))
                {
                    aiController.occupationIndex = Mathf.Min(aiController.occupationIndex, i);
                    break;
                }
            }
        }

        foreach (var col in enemyCols)
        {
            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            if (aiController.priorityByOccupation[aiController.occupationIndex].SetTargetByPriority(aiController, colStatus))
            {
                //Debug.Log($"Filterd : {col.name}");

                if (aiController.battleTarget == null)
                {
                    aiController.battleTarget = col.transform;
                    target = col.transform;
                }

                if (aiController.priorityByDistance.SetTargetByPriority(aiController, colStatus))
                {
                    target = col.transform;
                }
            }
        }
        if (target != null)
        {
            //Debug.Log($"Result : {target.name}");
            aiController.SetBattleTarget(target);
            aiController.SetState(States.Trace);
            return;
        }
    }

    // Å½Áö¹üÀ§ Å½»ö
    protected void SearchTargetInDetectionRange()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.detectionRange, aiController.enemyLayer);
        if (enemyCols.Length == 0 && aiController.battleTarget == null)
        {
            //aiController.SetBattleTarget(aiController.point);
            aiController.SetState(States.MissionExecution);
            return;
        }

        Transform target = null;

        foreach (var col in enemyCols)
        {
            var dirToTarget = col.transform.position - aiTr.position;
            dirToTarget.Normalize();
            if (Physics.Raycast(aiTr.position, dirToTarget, aiStatus.sight, aiController.obstacleLayer))
                continue;

            //Debug.Log($"Col : {col.name}");


            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            for (int i = 0; i < aiController.priorityByOccupation.Count; ++i)
            {
                if (aiController.priorityByOccupation[i].SetTargetByPriority(aiController, colStatus))
                {
                    aiController.occupationIndex = Mathf.Min(aiController.occupationIndex, i);
                    break;
                }
            }

        }

        foreach (var col in enemyCols)
        {
            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            if (aiController.priorityByOccupation[aiController.occupationIndex].SetTargetByPriority(aiController, colStatus))
            {
                //Debug.Log($"Filterd : {col.name}");

                if (aiController.battleTarget == null)
                {
                    aiController.battleTarget = col.transform;
                    target = col.transform;
                }

                if (aiController.priorityByDistance.SetTargetByPriority(aiController, colStatus))
                {
                    target = col.transform;
                }
            }
        }

        if (target != null)
        {
            //Debug.Log($"Result : {target.name}");
            aiController.SetBattleTarget(target);
            aiController.SetState(States.Trace);
            return;
        }
    }
}
