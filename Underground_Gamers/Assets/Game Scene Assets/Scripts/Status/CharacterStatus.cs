using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    [Header("우선 순위 설정")]
    public OccupationType occupationType = OccupationType.None;
    public DistancePriorityType distancePriorityType = DistancePriorityType.None;

    public string AIName;

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

    [Header("리워드 목록")]
    public GameObject ai;
    public Sprite illustration;
    public Sprite aiClass;
    public int lv;
    public int maxLv;
    public float xp;
    public float maxXp;
    public Sprite grade;

    [Header("데미지 그래프")]
    public float dealtDamage;
    public float takenDamage;

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

    public void LevelUp()
    {
        var pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        while (xp >= maxXp)
        {
            lv++;
            if (lv >= maxLv)
            {
                xp = 0;
                if (maxLv >= 50)
                {
                    maxXp = 0;
                    break;
                }
                else
                {
                    maxXp = pt.GetLevelUpXp(lv + 1);
                }
            }
            else
            {
                xp = xp - maxXp;
                maxXp = pt.GetLevelUpXp(lv + 1);
            }
        }
    }

}
