using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBattleLayoutSlot : DragSlot
{
    public GameObject ghostImagePrefab;
    private GameObject ghostImage;

    private Transform uiCanvas;
    private BattleLayoutForge battleLayoutForge;

    private void Awake()
    {
        battleLayoutForge = GameObject.FindGameObjectWithTag("BattleLayoutForge").GetComponent<BattleLayoutForge>();
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Transform>();
        ghostImage = Instantiate(ghostImagePrefab, uiCanvas);
        ghostImage.SetActive(false);
        battleLayoutForge.AddSlot(this);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        List<DragSlot> slots = battleLayoutForge.GetSlots();
        foreach(DragBattleLayoutSlot slot in slots)
        {
            slot.SetActiveAllRayCast(false);
        }
        SetActiveAllRayCast(false);
        ghostImage.SetActive(true);
        ghostImage.transform.position = eventData.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        ghostImage.transform.position = eventData.position;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        List<DragSlot> slots = battleLayoutForge.GetSlots();
        foreach (DragBattleLayoutSlot slot in slots)
        {
            slot.SetActiveAllRayCast(true);
        }
        ghostImage.SetActive(false);
    }


    public void SetActiveAllRayCast(bool isActive)
    {
        var images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            image.raycastTarget = isActive;
        }
    }    
   
}
