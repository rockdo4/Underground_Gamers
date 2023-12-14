using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<CharacterStatus> pcBuildings;
    public List<CharacterStatus> npcBuildings;

    public void ReleaseBuilding(TeamIdentifier building)
    {
        var buildingStatus = building.GetComponent<CharacterStatus>();
        switch (building.teamType)
        {
            case TeamType.PC:
                pcBuildings.Remove(buildingStatus);
                break;
            case TeamType.NPC:
                npcBuildings.Remove(buildingStatus);
                break;
        }
    }

    public bool IsPlayerWinByTimeOut()
    {
        int pcTotalHp = 0;
        int npcTotalHp = 0;

        int pcRemainedCount = 0;
        int npcRemainedCount = 0;
        pcRemainedCount = pcBuildings.Count;
        npcRemainedCount = npcBuildings.Count;
        foreach (CharacterStatus building in pcBuildings)
        {
            pcTotalHp += building.Hp;
        }        
        
        foreach (CharacterStatus building in npcBuildings)
        {
            npcTotalHp += building.Hp;
        }

        if(pcRemainedCount > npcRemainedCount)
        {
            return true;
        }
        else if(pcRemainedCount < npcRemainedCount)
        {
            return false;
        }

        return pcTotalHp >= npcTotalHp;
    }
}
