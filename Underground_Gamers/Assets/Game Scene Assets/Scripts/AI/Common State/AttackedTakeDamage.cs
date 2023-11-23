using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        CharacterStatus status = transform.GetComponent<CharacterStatus>();

        if (!status.IsLive)
            return;

        status.Hp -= attack.Damage;
        status.Hp = Mathf.Max(0, status.Hp);
        Debug.Log(status.Hp);

        if(status.Hp <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
