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

    // 부채꼴 탐색
    protected void SearchTargetInSector()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.sight, aiController.enemyLayer);
        //if (enemyCols.Length == 0 && aiController.battleTarget == null)
        //{
        //    //aiController.SetBattleTarget(aiController.point);
        //    aiController.SetState(States.MissionExecution);
        //    return;
        //}
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

            // 우선순위 기준 선택
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

        // 우선순위로 타겟 필터링
        foreach (var col in enemyCols)
        {
            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            TeamIdentifier colIdentity = col.GetComponent<TeamIdentifier>();
            // 전투중일때는 건물 탐색 제외
            if (colIdentity != null && colIdentity.isBuilding && aiController.isBattle)
                continue;
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

        aiController.occupationIndex = int.MaxValue;

        if (target != null)
        {
            //Debug.Log($"Result : {target.name}");
            aiController.SetBattleTarget(target);
            aiController.SetState(States.Trace);
            return;
        }
    }

    // 탐지범위 탐색
    protected void SearchTargetInDetectionRange()
    {
        var enemyCols = Physics.OverlapSphere(aiTr.position, aiStatus.detectionRange, aiController.enemyLayer);
        //if (enemyCols.Length == 0 && aiController.battleTarget == null)
        //{
        //    //aiController.SetBattleTarget(aiController.point);
        //    aiController.SetState(States.MissionExecution);
        //    return;
        //}

        Transform target = null;

        foreach (var col in enemyCols)
        {
            var dirToTarget = col.transform.position - aiTr.position;
            dirToTarget.Normalize();
            if (Physics.Raycast(aiTr.position, dirToTarget, aiStatus.sight, aiController.obstacleLayer))
                continue;

            //Debug.Log($"Col : {col.name}");

            // 우선순위 기준 선택

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

        // 우선순위로 타겟 필터링
        foreach (var col in enemyCols)
        {
            CharacterStatus colStatus = col.GetComponent<CharacterStatus>();
            TeamIdentifier colIdentity = col.GetComponent<TeamIdentifier>();
            // 전투중일때는 건물 탐색 제외
            if (colIdentity != null && colIdentity.isBuilding && aiController.isBattle)
                continue;
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

        aiController.occupationIndex = int.MaxValue;

        if (target != null)
        {
            //Debug.Log($"Result : {target.name}");
            aiController.SetBattleTarget(target);
            aiController.SetState(States.Trace);
            return;
        }
    }
}
