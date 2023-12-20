using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLayoutForge : MonoBehaviour
{
    private List<DragSlot> slots = new List<DragSlot>();

    public void AddSlot(DragSlot slot)
    {
        slots.Add(slot);
    }

    public List<DragSlot> GetSlots()
    {
        return slots;
    }
}
