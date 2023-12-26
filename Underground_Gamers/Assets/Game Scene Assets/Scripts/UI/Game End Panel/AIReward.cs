using DG.Tweening;
using DG.Tweening.Core.Easing;
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
    public GameObject levelUpPrefab;
    public Transform levelUpPoint;

    public float currentXP;
    public float maxXP;
    public float maxLv;
    public int currentLv;
    private int completedTweenCount = 0;
    private int loopCount = 0;

    private float levelUpXpTime;
    private float remainedXpTime = 0.3f;
    public void DisplayXpGauge(float value)
    {
        xpGauge.value = value;
    }

    public void DisplayGetXp(int getXp)
    {
        xpText.text = $"Xp + {getXp}";
    }

    public void CalXp()
    {
        currentXP = ai.playerInfo.xp;
        maxXP = ai.playerInfo.maxXp;
        DisplayXpGauge(currentXP / maxXP);
    }

    public void DisplayLevel(int level)
    {
        currentLv = level;
        lvText.text = $"Lv. {currentLv}";
    }
    public void FillXpGauge(int loopCount, float duration)
    {
        levelUpXpTime = (duration - remainedXpTime) / loopCount;
        this.loopCount = loopCount;
        if (this.loopCount > completedTweenCount)
        {
            completedTweenCount++;
            xpGauge.DOValue(1, levelUpXpTime).SetEase(Ease.InOutQuint).OnComplete(TweenCompleted);
        }
        else
        {
            DisplayRemainedXp();
        }
    }
    public void TweenCompleted()
    {
        xpGauge.value = 0f;
        DisplayLevel(currentLv + loopCount - (loopCount - completedTweenCount));
        CreateLevelUpImage();
        if (loopCount > completedTweenCount)
        {
            // 레벨업 표시하기
            xpGauge.DOValue(1, levelUpXpTime).SetEase(Ease.InOutQuint).OnComplete(() => TweenCompleted());
            completedTweenCount++;

        }
        else
        {
            completedTweenCount = 0;
            levelUpXpTime = 0;
            DisplayRemainedXp();
        }
    }

    public void CreateLevelUpImage()
    {
        GameObject levelUpImage = Instantiate(levelUpPrefab, levelUpPoint);
    }

    public void DisplayRemainedXp()
    {
        xpGauge.DOValue(ai.playerInfo.xp / ai.playerInfo.maxXp, remainedXpTime).SetEase(Ease.InOutQuint).OnComplete(CalXp);
    }

    void AdditionalTween()
    {
        xpGauge.DOValue(1, 1.0f).SetEase(Ease.InOutQuint); // 추가적인 Tweener 실행
    }
}

