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
        Quaternion targetRotation = Quaternion.LookRotation(aiController.target.position - aiTr.position);
        aiTr.rotation = Quaternion.RotateTowards(aiTr.rotation, targetRotation, aiStatus.reactionSpeed * Time.deltaTime);
        //Debug.Log($"THIS {this.target} ");
        //Debug.Log($"TARGET {target} ");

    }
}
