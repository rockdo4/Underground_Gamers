using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDropBattleLayoutPanel : OnDropPanel
{
    public Transform parent;
    public override void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.gameObject.transform.SetParent(parent);

        // Á¤·Ä
        var childs = parent.GetComponentsInChildren<DragBattleLayoutSlot>();
        Array.Sort(childs, CompareByAINum);
        foreach (var child in childs)
        {
            int index = child.AI.aiIndex;
            child.transform.SetSiblingIndex(index);
        }
    }

    private int CompareByAINum(DragBattleLayoutSlot a, DragBattleLayoutSlot b)
    {
        return a.AI.aiIndex.CompareTo(b.AI.aiIndex);
    }
}
