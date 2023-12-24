using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLayoutForge : MonoBehaviour
{
    public List<DragBattleLayoutSlot> Slots { get; private set; } = new List<DragBattleLayoutSlot>();
    public SelectLine selectLineButton;
    public void AddSlot(DragBattleLayoutSlot slot)
    {
        Slots.Add(slot);
    }

    public void ClearSlot()
    {
        foreach(var slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }
        Slots.Clear();
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
