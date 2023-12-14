using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Vector3 prevPos;
    private CommandManager commandManager;
    public bool isDragging;

    public static GameObject dragInfo = null;
    private static Image topDropPanel;
    private static Image bottomDropPanel;

    private void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag("CommandManager").GetComponent<CommandManager>();
        topDropPanel = GameObject.FindGameObjectWithTag("TopDropPanel").GetComponent<Image>();
        bottomDropPanel = GameObject.FindGameObjectWithTag("BottomDropPanel").GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        prevPos = transform.position;
        transform.position = eventData.position;
        GetComponent<Image>().raycastTarget = false;
        dragInfo = this.gameObject;

        OnRaycastDropPanel();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragAndDrop.OffRaycastDropPanel();

        isDragging = false;
        transform.position = prevPos;
        dragInfo = null;
    }

    public void OnRaycastDropPanel()
    {
        topDropPanel.raycastTarget = true;
        bottomDropPanel.raycastTarget = true;
    }
    public static void OffRaycastDropPanel()
    {
        topDropPanel.raycastTarget = false;
        bottomDropPanel.raycastTarget = false;
    }
}
