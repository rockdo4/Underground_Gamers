using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIReward : MonoBehaviour
{
    public CharacterStatus aiStatus;
    public Image illustration;
    public Image aiClass;
    public TextMeshProUGUI lvText;
    public Image grade;
    public TextMeshProUGUI aiNameText;

    public Slider xpGauge;
    public TextMeshProUGUI xpText;
    public float currentXP;
    public float maxXP;
    public float maxLv;


    public void DisplayExpGauge(float value)
    {
        xpGauge.value = value;
    }

    public void CalXp()
    {
        DisplayExpGauge(currentXP / maxXP);
    }
}
