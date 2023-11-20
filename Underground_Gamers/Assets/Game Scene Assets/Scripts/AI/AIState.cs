using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : BaseState
{
    private AIController aiController;
    private NavMeshAgent agent;
    private Animator animator;

    public AIState(AIController aiController)
    {
        this.aiController = aiController.GetComponent<AIController>();
        agent = aiController.GetComponent<NavMeshAgent>();
        animator = aiController.GetComponent<Animator>();
    }
}
