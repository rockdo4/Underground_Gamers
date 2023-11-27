using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        CharacterStatus status = transform.GetComponent<CharacterStatus>();
        AIController controller = transform.GetComponent<AIController>();
        AIController attackerAI = null;

        if (attacker != null)
        {
            attackerAI = attacker.GetComponent<AIController>();
        }
        if (!status.IsLive)
            return;

        status.Hp -= attack.Damage;
        status.Hp = Mathf.Max(0, status.Hp);

        if (status.Hp <= 0)
        {
            status.IsLive = false;
            if (attackerAI != null)
                attackerAI.target = null;
            if (gameObject.layer == LayerMask.GetMask("PC"))
            {
                GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>().pc.Remove(controller);
            }
            else
            {
                GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>().npc.Remove(controller);
            }

            var destroyable = transform.GetComponent<IDestroyable>();
            var respawnable = transform.GetComponent<IRespawnable>();
            if (destroyable == null || respawnable == null)
                return;
            destroyable.DestoryObject();
            respawnable.ExecuteRespawn(transform.gameObject);

            //Destroy(transform.gameObject);
        }
    }
}
