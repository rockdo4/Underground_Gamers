using UnityEngine;
using UnityEngine.UI;

public class DamageGraph : MonoBehaviour
{
    public float dealtDamage;
    public float takenDamage;
    private float maxDealtDamage;
    private float minDealtDamage;

    public Slider dealtDamageGauage;
    public Slider takenDamageGauage;
}
