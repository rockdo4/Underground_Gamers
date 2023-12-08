using TMPro;
using UnityEngine;

public class DisplayDamageText : MonoBehaviour, IAttackable
{
    public TextMeshPro damageText;
    public float OffsetText = 5f;
    public void OnAttack(GameObject attacker, Attack attack)
    {
        Vector3 pos = transform.position;
        pos.y += OffsetText;

        TextMeshPro text = Instantiate(damageText, pos, Quaternion.identity);
        if (attack.IsCritical)
        {
            text.color = Color.yellow;
            text.fontSize += 8f;
            Debug.Log("Critical");
        }
        text.text = attack.Damage.ToString();
    }
}
