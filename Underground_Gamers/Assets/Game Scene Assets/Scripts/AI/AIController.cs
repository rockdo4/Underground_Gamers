using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum States
{
    Idle,
    Trace,
    Attack,
}

public enum SkillTypes
{
    Base,                    // 기본공격
    General,                // 공용스킬
    Original,                // 고유스킬
    Count
}

public enum SkillActionTypes
{
    Active,
    Passive,
}
public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;
    public CharacterStatus status;

    private StateManager stateManager;
    private List<BaseState> states = new List<BaseState>();

    public Transform point;
    public Transform target;
    public Transform firePos;
    public LayerMask layer;
    public SPUM_Prefabs spum;

    public Transform rightHand;
    public Transform leftHand;
    public AIManager manager;

    public Vector3 hitInfoPos;



    public AttackDefinition[] attackInfos = new AttackDefinition[(int)SkillTypes.Count];


    [Tooltip("탐지 딜레이 시간")]
    public float detectTime = 0.1f;

    public int teamLayer;
    public int enemyLayer;
    public int obstacleLayer;


    public bool RaycastToTarget
    {
        get
        {
            var origin = firePos.position;
            //origin.y += 0.6f;
            var target = this.target.position;
            target.y += firePos.position.y;
            var direction = target - origin;
            direction.Normalize();

            //var layer = 0xFFFF ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

            //int mask = LayerMask.GetMask(
            //LayerMask.LayerToName(gameObject.layer),
            //    LayerMask.LayerToName(this.target.gameObject.layer));
            //var layer = Physics.AllLayers ^ mask;
            bool isCol = Physics.Raycast(origin, direction, out RaycastHit hitInfo, status.range, enemyLayer);

            if (isCol)
            {
                hitInfoPos = hitInfo.point;
            }
            return isCol;
        }
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // 데이터 테이블에 따라서 정보를 가져올 수 있음, 레벨 정보는 세이브 데이터
        status = GetComponent<CharacterStatus>();
        target = point;

        teamLayer = layer;

        if (teamLayer == LayerMask.GetMask("PC"))
            enemyLayer = LayerMask.GetMask("NPC");
        else
            enemyLayer = LayerMask.GetMask("PC");

        obstacleLayer = LayerMask.GetMask("Obstacle");

        firePos = rightHand;
    }

    private void Start()
    {
        stateManager = new StateManager();
        states.Add(new IdleState(this));
        states.Add(new TraceState(this));
        states.Add(new AttackState(this));

        agent.speed = status.speed;
        agent.SetDestination(point.position);

        SetState(States.Idle);
    }

    public void SetDestination(Vector3 vector3)
    {
        agent.SetDestination(vector3);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(point.position);
        }
        spum._anim.SetFloat("RunState", Mathf.Min(agent.velocity.magnitude,0.5f));
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

        if(firePos != null && target != null)
        {
            if (RaycastToTarget)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(firePos.position, hitInfoPos);
            }
        }
    }
}
