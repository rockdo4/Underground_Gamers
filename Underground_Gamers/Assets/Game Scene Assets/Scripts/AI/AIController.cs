using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterStatus status;
    private StateManager stateManager;
    private List<BaseState> states = new List<BaseState>();

    private LookCamera lookCamera;
    public Transform point;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<CharacterStatus>();
        lookCamera = GetComponentInChildren<LookCamera>();
    }

    private void Start()
    {
        stateManager = new StateManager();
        states.Add(new IdleState(this));
        agent.speed = status.speed;
        agent.SetDestination(point.position);
    }

    public void SetState(Enums.States newState)
    {
        stateManager.ChangeState(states[(int)newState]);
    }    
    private void Update()
    {
        lookCamera.LookAtCamera();
    }

    private void UpdateState()
    {

    }
}
