using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<CharacterStatus> pcBuildings;
    public List<CharacterStatus> npcBuildings;

    public GameType gameType;
    public int PCWinEvidenceCount { get; private set; } = 0;
    public int NPCWinEvidenceCount { get; private set; } = 0;
    public int WinningCount { get; private set; } = 1;

    public List<WinEvidence> pcWinEvidences = new List<WinEvidence>();
    public List<WinEvidence> npcWinEvidences = new List<WinEvidence>();

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

        if (pcRemainedCount > npcRemainedCount)
        {
            return true;
        }
        else if (pcRemainedCount < npcRemainedCount)
        {
            return false;
        }

        return pcTotalHp >= npcTotalHp;
    }
    public void SetGameType(GameType gameType)
    {
        this.gameType = gameType;
        WinningCount = this.gameType switch
        {
            GameType.Story => 1,
            GameType.Official => 2,
            GameType.Scrimmage => 1,
            _ => 1
        };

        switch (gameType)
        {
            case GameType.Story:
                SetActiveWinEvidence(false);
                break;
            case GameType.Official:
                SetActiveWinEvidence(true);
                break;
            case GameType.Scrimmage:
                SetActiveWinEvidence(false);
                break;
        }
    }

    public int GetWinEvidence(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            FillWinEvidence(pcWinEvidences, ++PCWinEvidenceCount);
            return PCWinEvidenceCount;
        }
        else
        {
            FillWinEvidence(npcWinEvidences, ++NPCWinEvidenceCount);
            return NPCWinEvidenceCount;
        }
    }

    public void SetActiveWinEvidence(bool isActive)
    {
        foreach (var evidence in pcWinEvidences)
        {
            evidence.SetActiveWinEvidence(isActive);
        }
        foreach (var evidence in npcWinEvidences)
        {
            evidence.SetActiveWinEvidence(isActive);
        }
    }

    public void FillWinEvidence(List<WinEvidence> winEvidences, int winEvidenceCount)
    {
        winEvidences[winEvidenceCount - 1].FillWinEvidence(true);
    }
}
