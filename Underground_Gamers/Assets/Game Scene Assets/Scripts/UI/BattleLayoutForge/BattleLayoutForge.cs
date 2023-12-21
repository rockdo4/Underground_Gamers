using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLayoutForge : MonoBehaviour
{
    private List<DragBattleLayoutSlot> slots = new List<DragBattleLayoutSlot>();
    public SelectLine selectLineButton;
    public void AddSlot(DragBattleLayoutSlot slot)
    {
        slots.Add(slot);
    }

    public List<DragBattleLayoutSlot> GetSlots()
    {
        return slots;
    }

    public void SetActiveSelectLineButton(bool isActive)
    {
        selectLineButton.SetActive(isActive);
    }

    public void SetActiveBattleLayoutForge(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}