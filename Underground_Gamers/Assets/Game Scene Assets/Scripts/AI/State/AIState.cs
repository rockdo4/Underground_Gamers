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

    protected Transform target;
    protected int teamLayer;
    protected int enemyLayer;
    protected int obstacleLayer;


    public AIState(AIController aiController)
    {
        this.aiController = aiController.GetComponent<AIController>();
        aiStatus = aiController.GetComponent<CharacterStatus>();
        agent = aiController.GetComponent<NavMeshAgent>();
        animator = aiController.GetComponent<Animator>();
        aiTr = aiController.GetComponent<Transform>();
        teamLayer = aiController.layer;

        if (teamLayer == LayerMask.GetMask("PC"))
            enemyLayer = LayerMask.GetMask("NPC");
        else
            enemyLayer = LayerMask.GetMask("PC");

        obstacleLayer = LayerMask.GetMask("Obstacle");
        target = aiController.point;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.SetDestination(this.target.position);

    }
}
