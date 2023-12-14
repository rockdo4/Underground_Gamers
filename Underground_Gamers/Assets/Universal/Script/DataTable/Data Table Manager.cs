using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DataTableManager : MonoBehaviour
{
    public PlayersStateDef stateDef;
    public static DataTableManager instance
    {
        get
        {
            if (dataTableManager == null)
            {
                dataTableManager = FindObjectOfType<DataTableManager>();
            }
            return dataTableManager;
        }
    }

    private static DataTableManager dataTableManager;

    Dictionary<DataType, DataTable> tables;

    private void Awake()
    {
        LoadAll();
    }

    public void LoadAll()
    {
        tables = new Dictionary<DataType, DataTable>
        {
            { DataType.String, new StringTable() },
            { DataType.Player, new PlayerTable() },
            { DataType.Gear, new GearTable() },
            { DataType.Recruit, new RecruitTable() },
            { DataType.Stage, new StageTable() },
        };
        foreach (var table in tables)
        {
            table.Value.DataAdder();
        }
    }

    public T Get<T>(DataType type)
    {
        foreach (var table in tables) 
        {
            if (table.Key == type)
            {
                return (T)Convert.ChangeType(table.Value, typeof(T));
            }
        }
        return default;
    }
}

