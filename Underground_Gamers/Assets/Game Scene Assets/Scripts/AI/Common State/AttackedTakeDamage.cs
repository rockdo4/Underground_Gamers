using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        CharacterStatus status = transform.GetComponent<CharacterStatus>();
        AIController controller = transform.GetComponent<AIController>();

        if (!status.IsLive)
            return;

        // 막기 버프 적용
        if (controller != null)
        {
            if (controller.isInvalid)
            {
                foreach (var buff in controller.appliedBuffs)
                {
                    if (buff is InvalidAttackBuff)
                    {
                        InvalidAttackBuff invalidAttackBuff = buff as InvalidAttackBuff;
                        invalidAttackBuff.invalidAttackCount--;
                        if (invalidAttackBuff.invalidAttackCount <= 0)
                        {
                            invalidAttackBuff.RemoveBuff(controller);
                        }
                        return;
                    }
                }
            }
        }

        status.Hp -= attack.Damage;
        status.Hp = Mathf.Min(status.Hp, status.maxHp);
        status.Hp = Mathf.Max(0, status.Hp);

        // 반격
        if (controller != null)
        {
            TeamIdentifier targetIdentity = controller.battleTarget.GetComponent<TeamIdentifier>();
            if (controller.battleTarget != null)
            {
                if (!controller.isBattle || controller.battleTarget == null || targetIdentity.isBuilding)
                {
                    controller.SetState(States.Trace);
                    controller.SetBattleTarget(attacker.transform);
                }
            }
        }

        // 사망 처리
        if (status.Hp <= 0)
        {
            status.IsLive = false;
            TeamIdentifier identity = transform.GetComponent<TeamIdentifier>();
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

            var destroyables = transform.GetComponents<IDestroyable>();
            var respawnables = transform.GetComponents<IRespawnable>();
            if (destroyables != null)
            {
                foreach (var destroyable in destroyables)
                {
                    destroyable.DestoryObject();
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
