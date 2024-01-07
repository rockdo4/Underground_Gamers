using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayKillLog : MonoBehaviour, IDestroyable
{
    public KillLogPanel killLogPanel;
    public KillLog killLog;
    public GameObject killerPortrait;
    public GameObject deadPortrait;

    private void Start()
    {
        killLogPanel = GameObject.FindGameObjectWithTag("KillLogPanel").GetComponent<KillLogPanel>();
    }

    public void DestoryObject(GameObject attacker)
    {
        var attackerInfo = attacker.GetComponent<Portrait>();
        var attackerIdentity = attacker.GetComponent<TeamIdentifier>();
        var deadInfo = transform.GetComponent<Portrait>();
        var attackerPortrait = attackerInfo.GetPortrait();
        var deadPortrait = deadInfo.GetPortrait();

        KillLog killLog = Instantiate(this.killLog, killLogPanel.transform);
        killLog.transform.SetParent(killLogPanel.transform);
        killLog.transform.localRotation = Quaternion.identity;

        Vector3 killLogPos = killLog.transform.localPosition;
        killLogPos.z = 0f;
        killLog.transform.localPosition = killLogPos;
        killLog.transform.localScale = Vector3.one;
        killLog.destroyedTimer = Time.time;
        killLogPanel.killLogs.Add(killLog);
        // 킬로그 리프레시, 3개 제한
        if (killLogPanel.killLogs.Count > 3)
        {
            killLogPanel.RefreshKillLogPanel();
        }

        // 초상화 세팅
        if (attackerPortrait != null && deadPortrait != null)
        {
            killLog.SetKillLog(attackerPortrait, deadPortrait, attackerIdentity);
        }
    }
}
