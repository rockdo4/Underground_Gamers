using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatusDebug : MonoBehaviour
{
    public AIManager aiManager;
    public DebugAIStatusInfo aiInfoPrefab;

    public Transform pcPannel;
    public Transform npcPannel;

    private Transform pcAIInfoParent;
    private Transform npcAIInfoParent;

    private void Awake()
    {
        pcAIInfoParent = pcPannel.GetChild(0);
        npcAIInfoParent = npcPannel.GetChild(0);
        int pcNum = 1;
        int npcNum = 1;

        foreach(var ai in aiManager.pc)
        {
            ai.debugAIStatusInfo = Instantiate(aiInfoPrefab, pcAIInfoParent);
            ai.debugAIStatusInfo.aiType = "PC";
            ai.debugAIStatusInfo.aiNum = pcNum++;

        }
        foreach(var ai in aiManager.npc)
        {
            ai.debugAIStatusInfo = Instantiate(aiInfoPrefab, npcAIInfoParent);
            ai.debugAIStatusInfo.aiType = "NPC";
            ai.debugAIStatusInfo.aiNum = npcNum++;
        }
    }
}
