using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageGraph : MonoBehaviour
{
    public float dealtDamage = 0;
    public float takenDamage = 0;
    public float healAmount = 0;

    private float maxDealtDamage = 0;
    private float maxTakenDamage = 0;
    private float maxHealAmount = 0;

    public Slider dealtDamageGauage;
    public Slider takenDamageGauage;
    public Slider healAmountGauage;

    public TextMeshProUGUI dealtDamageText;
    public TextMeshProUGUI takenDamageText;
    public TextMeshProUGUI healAmountText;

    public Image illustration;

    public void SetMaxDamages(float maxDealtDamage, float maxTakenDamage, float maxHealAmount)
    {
        this.maxDealtDamage = maxDealtDamage;
        this.maxTakenDamage = maxTakenDamage;
        this.maxHealAmount = maxHealAmount;
    }

    public void DisplayDamges()
    {
        dealtDamageText.text = $"{dealtDamage}";
        takenDamageText.text = $"{takenDamage}";
        healAmountText.text = $"{healAmount}";
        dealtDamageGauage.value = dealtDamage / maxDealtDamage;
        takenDamageGauage.value = takenDamage / maxTakenDamage;
        healAmountGauage.value = healAmount / maxHealAmount;
    }
}
