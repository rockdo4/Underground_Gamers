using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDropBattleLayoutPanel : OnDropPanel
{
    public GameManager gameManager;

    public Line line;
    public bool isAttack;
    public Transform parent;
    public override void OnDrop(PointerEventData eventData)
    {
        DragBattleLayoutSlot dragBattleLayoutSlot = eventData.pointerDrag.gameObject.GetComponent<DragBattleLayoutSlot>();
        dragBattleLayoutSlot.SetParent(parent);

        gameManager.entryManager.SetAIEntry(line, isAttack, dragBattleLayoutSlot);
        // Á¤·Ä
        var childs = parent.GetComponentsInChildren<DragBattleLayoutSlot>();
        Array.Sort(childs, CompareByAINum);
        foreach (var child in childs)
        {
            int index = child.AI.aiIndex;
            child.transform.SetSiblingIndex(index);
        }

        gameManager.entryManager.RefreshSelectLineButton();
    }

    private int CompareByAINum(DragBattleLayoutSlot a, DragBattleLayoutSlot b)
    {
        return a.AI.aiIndex.CompareTo(b.AI.aiIndex);
    }
}
