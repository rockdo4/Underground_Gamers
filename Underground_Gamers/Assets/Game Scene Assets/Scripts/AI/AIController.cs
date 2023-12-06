using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum OccupationType
{
    None,
    Normal,     // 일반형
    Assault,    // 돌격형
    Sniper,     // 저격형
    Support,     // 지원형
    Count
}

public enum DistancePriorityType
{
    None,
    Closer,     // 가까운
    Far,        // 먼
    Count
}


public enum States
{
    Idle,
    MissionExecution,
    Trace,
    AimSearch,
    Attack,
    Kiting,
    Reloading,
}

public enum SkillTypes
{
    Base,                    // 기본공격
    Original,                // 고유스킬
    General,                // 공용스킬
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
    public Line currentLine = Line.Bottom;


    public LayerMask layer;
    public SPUM_Prefabs spum;

    public Transform rightHand;
    public Transform leftHand;
    public AIManager manager;

    public Vector3 hitInfoPos;
    public Vector3 kitingPos;
    public bool isKiting = false;


    [Header("전투 상태")]
    public bool isBattle = false;
    public bool isTower = false;

    [Header("공격 & 카이팅 상태")]
    public AttackDefinition[] attackInfos = new AttackDefinition[(int)SkillTypes.Count];
    public KitingData kitingInfo;
    public KitingData reloadingKitingInfo;

    [Header("우선 순위 설정")]
    public OccupationType occupationType = OccupationType.None;
    public DistancePriorityType distancePriorityType = DistancePriorityType.None;
    public TargetPriority priorityByDistance;

    public List<TargetPriority> priorityByOccupation = new List<TargetPriority>();
    public List<TeamIdentifier> filteredByOccupation = new List<TeamIdentifier>();

    [Tooltip("탐지 딜레이 시간")]
    public float detectTime = 0.1f;

    [Header("공격, 스킬 관련")]
    [Tooltip("마지막 공격 시점")]
    public float lastBaseAttackTime;
    [Tooltip("공격 딜레이 타임")]
    public float baseAttackCoolTime;
    public bool isOnCoolBaseAttack;

    [Tooltip("마지막 고유스킬 사용 시점")]
    public float lastOriginalSkillTime;
    [Tooltip("고유스킬 딜레이 타임")]
    public float originalSkillCoolTime;
    public bool isOnCoolOriginalSkill;

    [Tooltip("마지막 공용스킬 사용 시점")]
    public float lastGeneralSkillTime;
    [Tooltip("공용스킬 딜레이 타임")]
    public float generalSkillCoolTime;
    public bool isOnCoolGeneralSkill = false;    
    
    [Tooltip("마지막 장전 시점")]
    public float lastReloadTime;
    [Tooltip("장전 타임")]
    public float reloadCoolTime;
    public bool isReloading;

    public int maxAmmo;
    public int currentAmmo;

    [Header("버프")]
    public List<Buff> appliedBuffs = new List<Buff>();
    public List<Buff> removedBuffs = new List<Buff>();
    public bool isInvalid = false;

    [Header("레이어")]
    public int teamLayer;
    public int enemyLayer;
    public int obstacleLayer;

    [Header("UI")]
    public string statusName;
    public DebugAIStatusInfo debugAIStatusInfo;
    public CommandInfo aiCommandInfo;
    public TextMeshProUGUI aiType;
    public int colorIndex;

    public AICanvas canvas;

    public Transform[] tops;
    public Transform[] bottoms;

    public bool RaycastToTarget
    {
        get
        {
            if (this.target == null)
                return false;
            var origin = transform.position;
            var target = this.target.position;

            var sightOrigin = transform.position;
            var sightTarget = this.target.position;
            sightTarget.y = sightOrigin.y;

            var sightDirection = sightTarget - sightOrigin;
            sightDirection.Normalize();

            var direction = target - origin;
            direction.Normalize();

            float sightDot = Vector3.Dot(sightDirection, transform.forward);

            float dot = Vector3.Dot(direction, transform.forward);
            float angleInRadians = Mathf.Acos(dot);
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            float cosineValue = Mathf.Abs(Mathf.Cos(angleInDegrees));
            float attackRange = status.range / cosineValue;

            float sightAngle = 1f - (status.sightAngle / 2) * 0.01f;

            bool isCol = false;
            if (sightDot > sightAngle)
            {
                isCol = Physics.Raycast(origin, direction, out RaycastHit hitInfo, attackRange, enemyLayer);
                if (isCol)
                {
                    hitInfoPos = hitInfo.point;
                }
            }

            return isCol;
        }
    }
    public float DistanceToTarget
    {
        get
        {
            if (target == null)
            {
                return 0f;
            }
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            return Vector3.Distance(transform.position, targetPos);
        }
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // 데이터 테이블에 따라서 정보를 가져올 수 있음, 레벨 정보는 세이브 데이터
        status = GetComponent<CharacterStatus>();
        target = point;

        SetInitialization();

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
        states.Add(new ReloadingState(this));

        agent.speed = status.speed;
        //agent.SetDestination(point.position);

        SetState(States.Idle);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(point.position);
        }

        if(lastOriginalSkillTime + originalSkillCoolTime < Time.time 
            && attackInfos[(int)SkillTypes.Original] != null)
        {
            lastOriginalSkillTime = Time.time;
            isOnCoolOriginalSkill = true;
        }

        if (lastBaseAttackTime + baseAttackCoolTime < Time.time)
        {
            lastBaseAttackTime = Time.time;
            isOnCoolBaseAttack = true;
        }

        if(isReloading)
        {
            float time = (1 - (Time.time - lastReloadTime ) / reloadCoolTime);
            GetReloadTime(time);
        }

        if(lastReloadTime + reloadCoolTime < Time.time && isReloading)
        {
            isReloading = false;
            Reload();
        }

        UpdateBuff();



        spum._anim.SetFloat("RunState", Mathf.Min(agent.velocity.magnitude, 0.5f));
        //최대 속도일때 0.5f가 되어야 함으로 2로나눔
    }

    public void SetTarget(Transform target)
    {
        Transform prevTarget = this.target;
        this.target = target;
        CharacterStatus status = target.GetComponent<CharacterStatus>();
        if (status != null)
        {
            if(prevTarget != null)
            {
                CharacterStatus prevTargetStatus = prevTarget.GetComponent<CharacterStatus>();
                TargetEventBus.Unsubscribe(prevTargetStatus, ReleaseTarget);
            }
            TargetEventBus.Subscribe(status, ReleaseTarget);
        }
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

    public void UpdateReloadKiting()
    {
        if (target != null)
            reloadingKitingInfo.UpdateKiting(target, this);
    }

    public void GetReloadTime(float time)
    {
        canvas.reloadBar.value = time;
    }

    public void TryReloading()
    {
        canvas.reloadBar.gameObject.SetActive(true);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        canvas.reloadBar.gameObject.SetActive(false);
    }

    public void RefreshDebugAIStatus(string debug)
    {
        statusName = $"{debugAIStatusInfo.aiType}{debugAIStatusInfo.aiNum} : {debug}";
        debugAIStatusInfo.GetComponentInChildren<TextMeshProUGUI>().text = statusName;
    }

    private void UpdateBuff()
    {
        foreach (var buff in appliedBuffs)
        {
            buff.UpdateBuff(this);
        }

        foreach (var buff in removedBuffs)
        {
            appliedBuffs.Remove(buff);
        }

        if (removedBuffs.Count > 0)
        {
            removedBuffs.Clear();
        }
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

    public void SetInitialization()
    {
        if (attackInfos[(int)SkillTypes.Base] != null)
        {
            baseAttackCoolTime = attackInfos[(int)SkillTypes.Base].cooldown;
            reloadCoolTime = attackInfos[(int)SkillTypes.Base].reloadCooldown;
            maxAmmo = attackInfos[(int)SkillTypes.Base].chargeCount;
        }
        lastReloadTime = Time.time - reloadCoolTime;
        lastBaseAttackTime = Time.time - baseAttackCoolTime;

        if (attackInfos[(int)SkillTypes.Original] != null)
            originalSkillCoolTime = attackInfos[(int)SkillTypes.Original].cooldown;
        lastOriginalSkillTime = Time.time - originalSkillCoolTime;

        if (attackInfos[(int)SkillTypes.General] != null)
            generalSkillCoolTime = attackInfos[(int)SkillTypes.General].cooldown;
        lastGeneralSkillTime = Time.time - generalSkillCoolTime;


        Reload();
    }
}
