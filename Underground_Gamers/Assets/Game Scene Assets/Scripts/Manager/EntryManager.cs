using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public BattleLayoutForge battleLayoutForge;
    public GameManager gameManager;

    public DragBattleLayoutSlot slotPrefab;
    public Transform entryPanel;

    public void SetEntry()
    {
        foreach(var ai in gameManager.aiManager.pc)
        {
            DragBattleLayoutSlot slot = Instantiate(slotPrefab, entryPanel);
            slot.MatchAI(ai);
            slot.MatchPortrait();
            Debug.Log(ai.aiIndex);
            battleLayoutForge.AddSlot(slot);
        }
        Time.timeScale = 0f;
    }
}
