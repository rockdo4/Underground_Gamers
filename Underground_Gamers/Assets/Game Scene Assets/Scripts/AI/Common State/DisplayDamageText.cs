using TMPro;
using UnityEngine;

public class DisplayDamageText : MonoBehaviour, IAttackable
{
    public TextMeshPro damageText;
    public float OffsetText = 5f;
    private Color shiledColor;
    private Color healColor;

    private void Awake()
    {
        shiledColor = new Color(0f, 0.5f, 0f);
        healColor = new Color(0f, 1f, 0f);
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        Vector3 pos = transform.position;
        pos.y += OffsetText;
        TextMeshPro text = Instantiate(damageText, pos, Quaternion.identity);

        var defenderController = GetComponent<AIController>();
        if (defenderController != null)
        {
            if (defenderController.isInvalid)
            {
                text.text = "Shield";
                text.color = shiledColor;
                return;
            }
        }

        if (attack.IsHeal)
        {
            text.color = healColor;
        }

        if (attack.IsCritical)
        {
            text.color = Color.yellow;
            text.fontSize += 8f;
        }
        float damage = attack.Damage;
        if (attack.IsHeal)
            damage *= -1;
        text.text = damage.ToString();
    }
}
