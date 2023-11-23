using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public enum States
{
    Idle,
    Trace,
    Attack,
}
public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    public CharacterStatus status;

    private StateManager stateManager;
    private List<BaseState> states = new List<BaseState>();

    public Transform point;
    public LayerMask layer;
    public SPUM_Prefabs spum;

    [Tooltip("탐지 딜레이 시간")]
    public float detectTime = 0.1f;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // 데이터 테이블에 따라서 정보를 가져올 수 있음, 레벨 정보는 세이브 데이터
        status = GetComponent<CharacterStatus>();
        spum.AddComponent<LookCameraRect>();
    }

    private void Start()
    {
        stateManager = new StateManager();
        states.Add(new IdleState(this));
        states.Add(new TraceState(this));

        agent.speed = status.speed;
        agent.SetDestination(point.position);

        SetState(States.Idle);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(point.position);
        }
        spum._anim.SetFloat("RunState", agent.velocity.magnitude/2f);
        //최대 속도일때 0.5f가 되어야 함으로 2로나눔
    }
    public void SetState(States newState)
    {
        stateManager.ChangeState(states[(int)newState]);
    }


    public void UpdateState()
    {
        stateManager.Update();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float viewAngle = 60f;
        float viewRadius = 2f;
        float dectionRange = 1f;
        if (status != null)
        {
            viewRadius = status.sight;
            viewAngle = status.sightAngle;
            dectionRange = status.detectionRange;
        }
        Gizmos.DrawWireSphere(transform.position, dectionRange);

        Vector3 viewAngleA = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 viewAngleB = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.blue;
        Vector3 newV = transform.position;
        newV.y += 0.5f;
        Gizmos.DrawLine(newV, newV + viewAngleA * viewRadius);
        Gizmos.DrawLine(newV, newV + viewAngleB * viewRadius);

    }
}
