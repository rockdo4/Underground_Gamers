using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        CharacterStatus defenderStatus = transform.GetComponent<CharacterStatus>();
        CharacterStatus attackerStatus = attacker.GetComponent<CharacterStatus>();
        AIController defenderController = transform.GetComponent<AIController>();
        AIController attackerController = attacker.GetComponent<AIController>();

        if (!defenderStatus.IsLive)
            return;

        // 막기 버프 적용
        if (defenderController != null)
        {
            if (defenderController.isInvalid)
            {
                foreach (var buff in defenderController.appliedBuffs)
                {
                    if (buff is InvalidAttackBuff)
                    {
                        InvalidAttackBuff invalidAttackBuff = buff as InvalidAttackBuff;
                        invalidAttackBuff.invalidAttackCount--;
                        if (invalidAttackBuff.invalidAttackCount <= 0)
                        {
                            invalidAttackBuff.RemoveBuff(defenderController);
                        }
                        return;
                    }
                }
            }
        }

        defenderStatus.Hp -= attack.Damage;
        defenderStatus.Hp = Mathf.Min(defenderStatus.Hp, defenderStatus.maxHp);
        defenderStatus.Hp = Mathf.Max(0, defenderStatus.Hp);
        defenderStatus.GetHp();

        // 데미지 그래프 수치 적용
        defenderStatus.takenDamage += attack.Damage;
        if (attack.Damage >= 0)
            attackerStatus.dealtDamage += attack.Damage;
        else
            attackerStatus.healAmount -= attack.Damage;

        // 반격, 수정
        if (defenderController != null/* && controller.battleTarget == null*/)
        {
            //controller.battleTarget = attacker.transform;
            //TeamIdentifier targetIdentity = controller.battleTarget.GetComponent<TeamIdentifier>();

            if (!defenderController.isBattle && !defenderController.isReloading/* || controller.battleTarget == null || targetIdentity.isBuilding*/)
            {
                defenderController.SetBattleTarget(attacker.transform);
                defenderController.SetState(States.Trace);
            }
        }

        TeamIdentifier identity = transform.GetComponent<TeamIdentifier>();

        // 건물 타겟 처리
        if (identity != null && identity.isBuilding)
        {
            identity.SetBuildingTarget(attacker.transform);
        }

        // 사망 처리
        if (defenderStatus.Hp <= 0)
        {
            defenderStatus.IsLive = false;
            if (!identity.isBuilding)
            {
                if (identity.teamLayer == LayerMask.GetMask("PC"))
                {
                    var text = GameObject.FindGameObjectWithTag("NPC_Score").GetComponent<TMP_Text>();
                    text.text = (int.Parse(text.text) + 1).ToString();
                }
                else
                {
                    var text = GameObject.FindGameObjectWithTag("PC_Score").GetComponent<TMP_Text>();
                    text.text = (int.Parse(text.text) + 1).ToString();
                }
                attackerStatus.killCount++;
                defenderStatus.deathCount++;

                if (attackerController.aiCommandInfo != null)
                    attackerController.aiCommandInfo.DisplayKillCount(attackerStatus.killCount, attackerStatus.deathCount);
                if (defenderController.aiCommandInfo != null)
                    defenderController.aiCommandInfo.DisplayKillCount(defenderStatus.killCount, defenderStatus.deathCount);
            }

            var destroyables = transform.GetComponents<IDestroyable>();
            var respawnables = transform.GetComponents<IRespawnable>();
            if (destroyables != null)
            {
                foreach (var destroyable in destroyables)
                {
                    destroyable.DestoryObject(attacker);
                }
            }
            if (respawnables != null)
            {
                foreach (var respawnable in respawnables)
                {
                    respawnable.ExecuteRespawn(transform.gameObject);
                }
            }
        }
    }
}
