using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScheduleUIManager : MonoBehaviour
{
    [Header("Schedule")]
    [SerializeField]
    private TMP_Text[] moneyT = new TMP_Text[2];
    public void OnEnter()
    {
        UpdateScheduleMoney();
    }
    public void UpdateScheduleMoney()
    {
        moneyT[0].text = GamePlayerInfo.instance.money.ToString();
        moneyT[1].text = GamePlayerInfo.instance.crystal.ToString();
    }
}
