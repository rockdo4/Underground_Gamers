using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleTablesGroup : MonoBehaviour
{
    [SerializeField]
    private List<ScheduleTable> scheduleTables = new List<ScheduleTable>();

    public void SetTables()
    {
        foreach (ScheduleTable scheduleTable in scheduleTables) 
        {
            scheduleTable.SetText();
        }
    }
}
