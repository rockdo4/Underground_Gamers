using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatusDebug : MonoBehaviour
{
    public AIManager aiManager;
    public AIStatusInfo aiInfoPrefab;

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
            ai.aiStatusInfo = Instantiate(aiInfoPrefab, pcAIInfoParent);
            ai.aiStatusInfo.aiType = "PC";
            ai.aiStatusInfo.aiNum = pcNum++;

        }
        foreach(var ai in aiManager.npc)
        {
            ai.aiStatusInfo = Instantiate(aiInfoPrefab, npcAIInfoParent);
            ai.aiStatusInfo.aiType = "NPC";
            ai.aiStatusInfo.aiNum = npcNum++;
        }
    }
}
