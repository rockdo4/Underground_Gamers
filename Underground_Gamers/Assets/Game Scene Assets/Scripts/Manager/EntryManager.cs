using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public BattleLayoutForge battleLayoutForge;
    public GameManager gameManager;

    public DragBattleLayoutSlot slotPrefab;
    public Transform entryPanel;

    public List<AIController> TopAttackEntryAI { get; private set; } = new List<AIController>();
    public List<AIController> TopDefendEntryAI { get; private set; } = new List<AIController>();
    public List<AIController> BottomAttackEntryAI { get; private set; } = new List<AIController>();
    public List<AIController> BottomDefendEntryAI { get; private set; } = new List<AIController>();
    public List<AIController> NoneEntryAI { get; private set; } = new List<AIController>();

    public Transform topAttackEntry;
    public Transform bottomAttackEntry;
    public Transform topDefendEntry;
    public Transform bottomDefendEntry;

    public void ResetBattleLayoutForge()
    {
        int index = 0;
        foreach (var ai in gameManager.aiManager.pc)
        {
            DragBattleLayoutSlot slot = Instantiate(slotPrefab, entryPanel);
            slot.MatchAI(ai, gameManager, index);
            slot.MatchPortrait();
            battleLayoutForge.AddSlot(slot);
            index++;
        }

        RefreshSelectLineButton();
    }    
    
    public void ResetBattleLayoutForge(List<int> indexs)
    {
        int index = 0;
        
        foreach (var ai in gameManager.aiManager.pc)
        {
            DragBattleLayoutSlot slot = Instantiate(slotPrefab, entryPanel);
            slot.MatchAI(ai, gameManager, indexs[index]);
            slot.MatchPortrait();
            battleLayoutForge.AddSlot(slot);
            index++;
        }

        RefreshSelectLineButton();
    }


    public void SetAIEntry(Line line, bool isAttack, DragBattleLayoutSlot slot)
    {
        slot.ReleaseEntry();
        switch (line)
        {
            case Line.Top:
                switch (isAttack)
                {
                    case true:
                        slot.RegistEntry(TopAttackEntryAI);
                        break;
                    case false:
                        slot.RegistEntry(TopDefendEntryAI);
                        break;
                }
                break;
            case Line.Bottom:
                switch (isAttack)
                {
                    case true:
                        slot.RegistEntry(BottomAttackEntryAI);
                        break;
                    case false:
                        slot.RegistEntry(BottomDefendEntryAI);
                        break;
                }
                break;
            case Line.None:
                slot.RegistEntry(NoneEntryAI);
                break;
        }
    }
    public void EnterGameByEntry()
    {
        foreach(var ai in TopAttackEntryAI)
        {
            ai.currentLine = Line.Top;
            ai.isAttack = true;
            ai.isDefend = false;
        }
        foreach(var ai in TopDefendEntryAI)
        {
            ai.currentLine = Line.Top;
            ai.isAttack = false;
            ai.isDefend = true;
        }
        foreach (var ai in BottomAttackEntryAI)
        {
            ai.currentLine = Line.Bottom;
            ai.isAttack = true;
            ai.isDefend = false;
        }
        foreach(var ai in BottomDefendEntryAI)
        {
            ai.currentLine = Line.Bottom;
            ai.isAttack = false;
            ai.isDefend = true;
        }
    }

    public void ClearEntry()
    {
        TopAttackEntryAI.Clear();
        TopDefendEntryAI.Clear();
        BottomAttackEntryAI.Clear();
        BottomDefendEntryAI.Clear();
        NoneEntryAI.Clear();
    }

    public void RefreshSelectLineButton()
    {
        if (NoneEntryAI.Count == 0)
        {
            gameManager.battleLayoutForge.SetActiveSelectLineButton(true);
        }
        else
        {
            gameManager.battleLayoutForge.SetActiveSelectLineButton(false);
        }
    }
}
