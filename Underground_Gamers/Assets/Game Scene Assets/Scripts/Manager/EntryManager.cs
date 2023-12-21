using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public BattleLayoutForge battleLayoutForge;
    public GameManager gameManager;

    public DragBattleLayoutSlot slotPrefab;
    public Transform entryPanel;

    private List<AIController> topAttackEntryAI = new List<AIController>();
    private List<AIController> topDefendEntryAI = new List<AIController>();
    private List<AIController> bottomAttackEntryAI = new List<AIController>();
    private List<AIController> bottomDefendEntryAI = new List<AIController>();
    public List<AIController> NoneEntryAI { get; private set; } = new List<AIController>();

    public void ResetEntry()
    {
        foreach (var ai in gameManager.aiManager.pc)
        {
            DragBattleLayoutSlot slot = Instantiate(slotPrefab, entryPanel);
            slot.MatchAI(ai, gameManager);
            slot.MatchPortrait();
            battleLayoutForge.AddSlot(slot);
        }

        RefreshSelectLineButton();
        Time.timeScale = 0f;
    }

    public void SetEntry()
    {
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
                        slot.RegistEntry(topAttackEntryAI);
                        break;
                    case false:
                        slot.RegistEntry(topDefendEntryAI);
                        break;
                }
                break;
            case Line.Bottom:
                switch (isAttack)
                {
                    case true:
                        slot.RegistEntry(bottomAttackEntryAI);
                        break;
                    case false:
                        slot.RegistEntry(bottomDefendEntryAI);
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
        foreach(var ai in topAttackEntryAI)
        {
            ai.currentLine = Line.Top;
            ai.isAttack = true;
            ai.isDefend = false;
        }
        foreach(var ai in topDefendEntryAI)
        {
            ai.currentLine = Line.Top;
            ai.isAttack = false;
            ai.isDefend = true;
        }
        foreach (var ai in bottomAttackEntryAI)
        {
            ai.currentLine = Line.Bottom;
            ai.isAttack = true;
            ai.isDefend = false;
        }
        foreach(var ai in bottomDefendEntryAI)
        {
            ai.currentLine = Line.Bottom;
            ai.isAttack = false;
            ai.isDefend = true;
        }
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
