using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogPanel : MonoBehaviour
{
    public List<KillLog> killLogs = new List<KillLog>();

    public void RefreshKillLogPanel()
    {
        KillLog oldKillLog = killLogs[0];
        killLogs.Remove(oldKillLog);
        Destroy(oldKillLog.gameObject);
    }
}
