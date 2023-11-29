using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DataType
{
    None = -1,
    String,
    Player,
    Gear,
    Count
}
public abstract class DataTable
{
    
    DataType tableType = DataType.None;
    public DataTable(DataType dataType)
    {
        tableType = dataType;
    }
    public abstract void DataAdder();
}
