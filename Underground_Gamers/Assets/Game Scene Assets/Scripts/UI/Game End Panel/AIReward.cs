using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIReward : MonoBehaviour
{
    public AIController ai;
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
    public int currentLv;


    public void DisplayXpGauge(float value)
    {
        xpGauge.value = value;
    }

    public void CalXp()
    {
        currentXP = ai.playerInfo.xp;
        maxXP = ai.playerInfo.maxXp;
        DisplayXpGauge(currentXP / maxXP);
    }

    public void DisplayLevel()
    {
        currentLv = ai.playerInfo.level;
        lvText.text = $"Lv. {currentLv}";
    }
    public void FillXpGauge(float duration)
    {
        xpGauge.DOValue(1.0f, duration); // DOTween을 사용하여 슬라이더를 채웁니다.
    }

    //public void FillXpGauge(float duration)
    //{
    //    //StartCoroutine(FillSliderOverTime(duration));
    //}
    //public IEnumerator FillXpGauge(float duration)
    //{
    //    float elapsedTime = 0f;
    //    float startValue = xpGauge.value;
    //    float endValue = 1.0f;

    //    while (elapsedTime < duration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        xpGauge.value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
    //        yield return null;
    //    }

    //    xpGauge.value = endValue; // 슬라이더를 최종 값으로 설정
    //}
}
