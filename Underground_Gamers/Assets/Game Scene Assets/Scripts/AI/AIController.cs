using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum States
{
    Idle,
    MissionExecution,
    Trace,
    AimSearch,
    Attack,
    Kiting,
}

public enum SkillTypes
{
    Base,                    // �⺻����
    General,                // ���뽺ų
    Original,                // ������ų
    Count
}

public enum SkillActionTypes
{
    Active,
    Passive,
}
public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
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
    public Vector3 kitingPos;
    public bool isKiting = false;

    public AttackDefinition[] attackInfos = new AttackDefinition[(int)SkillTypes.Count];
    public KitingData kitingInfo;

    [Tooltip("Ž�� ������ �ð�")]
    public float detectTime = 0.1f;

    [Tooltip("������ ���� ����")]
    public float lastBaseAttackTime;
    [Tooltip("���� ������ Ÿ��")]
    public float baseAttackCoolTime;
    public bool isOnCoolBaseAttack;

    public int teamLayer;
    public int enemyLayer;
    public int obstacleLayer;

    public string statusName;
    public DebugAIStatusInfo debugAIStatusInfo;
    public CommandInfo aiCommandInfo;
    public TextMeshProUGUI aiType;

    public bool RaycastToTarget
    {
        get
        {
            if (this.target == null)
                return false;
            var origin = transform.position;
            origin.y += 0.6f;
            var target = this.target.position;
            //target.y = transform.position.y;
            target.y = origin.y;
            var direction = target - origin;
            direction.Normalize();

            //var layer = 0xFFFF ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

            //int mask = LayerMask.GetMask(
            //LayerMask.LayerToName(gameObject.layer),
            //    LayerMask.LayerToName(this.target.gameObject.layer));
            //var layer = Physics.AllLayers ^ mask;

            float dot = Vector3.Dot(direction, transform.forward);
            float dotAngle = 1f - (status.sightAngle / 2) * 0.01f;

            bool isCol = false;
            if (dot > dotAngle)
            {
                isCol = Physics.Raycast(origin, direction, out RaycastHit hitInfo, status.range, enemyLayer);
                if (isCol)
                {
                    hitInfoPos = hitInfo.point;
                }
            }

            return isCol;
        }
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // ������ ���̺��� ���� ������ ������ �� ����, ���� ������ ���̺� ������
        status = GetComponent<CharacterStatus>();
        target = point;

        baseAttackCoolTime = attackInfos[(int)SkillTypes.Base].cooldown;
        lastBaseAttackTime = Time.time - baseAttackCoolTime;


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
        states.Add(new MissionExecutionState(this));
        states.Add(new TraceState(this));
        states.Add(new AimSearchState(this));
        states.Add(new AttackState(this));
        states.Add(new KitingState(this));

        agent.speed = status.speed;
        //agent.SetDestination(point.position);

        SetState(States.Idle);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        CharacterStatus status = target.GetComponent<CharacterStatus>();
        if(status != null)
            TargetEventBus.Subscribe(status, ReleaseTarget);
        SetDestination(this.target.position);
    }

    public void ReleaseTarget()
    {
        target = null;
    }

    public void SetDestination(Vector3 vector3)
    {
        agent.SetDestination(vector3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(point.position);
        }

        if (lastBaseAttackTime + baseAttackCoolTime < Time.time)
        {
            lastBaseAttackTime = Time.time;
            isOnCoolBaseAttack = true;
        }

        spum._anim.SetFloat("RunState", Mathf.Min(agent.velocity.magnitude, 0.5f));
        //�ִ� �ӵ��϶� 0.5f�� �Ǿ�� ������ 2�γ���
    }
    public void SetState(States newState)
    {
        stateManager.ChangeState(states[(int)newState]);
    }


    public void UpdateState()
    {
        stateManager.Update();
    }

    public void UpdateKiting()
    {
        if (target != null)
            kitingInfo.UpdateKiting(target, this);
    }

    public void RefreshDebugAIStatus(string debug)
    {
        statusName = $"{debugAIStatusInfo.aiType}{debugAIStatusInfo.aiNum} : {debug}";
        debugAIStatusInfo.GetComponentInChildren<TextMeshProUGUI>().text = statusName;
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

        if (firePos != null && target != null)
        {
            if (RaycastToTarget)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(firePos.position, hitInfoPos);
            }
        }
    }
}