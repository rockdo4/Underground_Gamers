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
    }
}
