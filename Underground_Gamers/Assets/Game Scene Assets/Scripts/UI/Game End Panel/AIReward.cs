using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIReward : MonoBehaviour
{
    public GameObject ai;
    public Image illustration;
    public Image aiClass;
    public TextMeshProUGUI lvText;
    public Image grade;
    public TextMeshProUGUI aiNameText;

    public Slider expGauage;
    public float currentEXP;
    public float maxEXP;

    public Transform aiParent;
}
