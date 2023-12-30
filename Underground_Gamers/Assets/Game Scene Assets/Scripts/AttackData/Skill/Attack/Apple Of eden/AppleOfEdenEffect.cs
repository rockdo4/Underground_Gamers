using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleOfEdenEffect : CreateEffectSkill
{
    private void OnTriggerEnter(Collider other)
    {
        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        CharacterStatus aStatus = controller.GetComponent<CharacterStatus>();
        CharacterStatus dStatus = other.GetComponent<CharacterStatus>();
        if (identity == null)
            return;
        if (other.gameObject.layer == controller.gameObject.layer)
            return;


        var attackables = other.GetComponentsInChildren<IAttackable>();

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(controller.gameObject, attack);
        }
    }
}
