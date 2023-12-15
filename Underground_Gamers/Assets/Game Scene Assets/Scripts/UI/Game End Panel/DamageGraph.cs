using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageGraph : MonoBehaviour
{
    public float dealtDamage;
    public float takenDamage;

    private float maxDealtDamage = 0;
    private float maxTakenDamage = 0;

    public Slider dealtDamageGauage;
    public Slider takenDamageGauage;

    public TextMeshProUGUI dealtDamageText;
    public TextMeshProUGUI takenDamageText;

    public Image illustration;

    public void SetMaxDamages(float maxDealtDamage, float maxTakenDamage)
    {
        this.maxDealtDamage = maxDealtDamage;
        this.maxTakenDamage = maxTakenDamage;
    }

    public void DisplayDamges()
    {
        dealtDamageText.text = $"{dealtDamage}";
        takenDamageText.text = $"{takenDamage}";
        dealtDamageGauage.value = dealtDamage / maxDealtDamage;
        takenDamageGauage.value = takenDamage / maxTakenDamage;

    }
}
