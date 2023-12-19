using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupOfficialSchedule : MonoBehaviour
{
    [SerializeField]
    private ScheduleTablesGroup[] scheduleTablesGroups = new ScheduleTablesGroup[4];
    private int currIndex = 0;
    private void OnEnable()
    {
        currIndex = GamePlayerInfo.instance.officialWeekNum switch
        {
            0 or 1 => 0,
            2 or 3 => 1,
            4 or 5 => 2,
            6 => 3,
            _ => 0,
        };
        OpenUp(currIndex);
    }

    private void OpenUp(int index)
    {
        currIndex = index;
        scheduleTablesGroups[index].SetTables();
        

        for (int i = 0; i < scheduleTablesGroups.Length; i++)
        {
            if (i == index)
            {
                scheduleTablesGroups[i].gameObject.SetActive(true);
            }
            else
            {
                scheduleTablesGroups[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenLeft()
    {
        OpenUp((currIndex - 1 + 4) % 4);
    }
    public void OpenRight()
    {
        OpenUp((currIndex + 1) % 4);
    }
}
