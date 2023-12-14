using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Header("우선 순위 설정")]
    public OccupationType occupationType = OccupationType.None;
    public DistancePriorityType distancePriorityType = DistancePriorityType.None;

    public string name;

    [Header("스텟")]
    public int maxHp = 50;
    public float speed;
    public float sight;                 // 시야
    public float sightAngle;        // 시야각
    public float detectionRange;    // 탐지범위

    public float range;         // 공격 사정거리
    //public float sharedSightRange;

    public float evasionRate;       // 회피율
    public float reactionSpeed;     // 반응속도

    public float damage;
    public float cooldown;
    public float critical;
    public int chargeCount;
    public float reloadCooldown;
    public float accuracyRate;

    public int armor;

    public int deathCount;
    public float respawnTime;
    public float respawnTimeIncreaseRate;
    public bool IsLive { get; set; } = true;
    public int Hp { get; set; }

    private void Awake()
    {
        Hp = maxHp;
    }

    public void Respawn()
    {
        AIController aiController = GetComponent<AIController>();
        GameManager gameManager = GetComponent<AIController>().gameManager;

        if (aiController != null)
        {
            if (aiController.attackInfos[(int)SkillTypes.Base] != null)
                aiController.isOnCoolBaseAttack = true;
            else
                aiController.isOnCoolBaseAttack = false;

            if (aiController.attackInfos[(int)SkillTypes.Original] != null)
                aiController.isOnCoolOriginalSkill = true;
            else
                aiController.isOnCoolOriginalSkill = false;

            if (aiController.attackInfos[(int)SkillTypes.General] != null)
                aiController.isOnCoolGeneralSkill = true;
            else
                aiController.isOnCoolGeneralSkill = false;

            //aIController.isKiting = false;
        }
        IsLive = true;
        Hp = maxHp;
        GetHp();

        // 상태 설정
        if (aiController.isAttack)
            aiController.SetState(States.MissionExecution);
        if (aiController.isDefend)
            aiController.SetState(States.Retreat);

        if(aiController.teamIdentity.teamType == TeamType.NPC)
        {
            gameManager.npcManager.SelectLineByRate(aiController);
            gameManager.lineManager.JoiningLine(aiController);
        }

        // 카메라 설정
        if(gameManager != null)
        {
            if(gameManager.commandManager.currentAI == aiController)
            {
                gameManager.cameraManager.StartZoomIn();
            }
        }
    }

    public void GetHp()
    {
        var aiCanvas = GetComponentInChildren<AICanvas>();
        if (aiCanvas != null)
        {
            aiCanvas.hpBar.value = (float)Hp / (float)maxHp;
        }
    }
}
