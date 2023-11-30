using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        CharacterStatus status = transform.GetComponent<CharacterStatus>();
        AIController controller = transform.GetComponent<AIController>();
        //AIController attackerAI = null;

        //if (attacker != null)
        //{
        //    attackerAI = attacker.GetComponent<AIController>();
        //}
        if (!status.IsLive)
            return;

        if(controller.isInvalid)
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

        status.Hp -= attack.Damage;
        status.Hp = Mathf.Max(0, status.Hp);


        if (status.Hp <= 0)
        {
            status.IsLive = false;



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

            //Destroy(transform.gameObject);
        }
    }

    public void InvalidAttack(AIController controller)
    {
    }
}
