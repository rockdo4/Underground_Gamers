using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManagerOfficialSchedule : MonoBehaviour
{
    [SerializeField]
    private GameObject[] OfficialSchedules = new GameObject[2];

    private void OnEnable()
    {
        bool isActive = GamePlayerInfo.instance.officialWeekNum < 7;
        OfficialSchedules[0].SetActive(isActive);
        OfficialSchedules[1].SetActive(!isActive);
    }
}
