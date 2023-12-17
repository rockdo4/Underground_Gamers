using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform canvas;
    private Vector3 prevPos;
    private CommandManager commandManager;
    public bool isDragging;
    public bool isDropSuccess = false;

    public static DragAndDrop dragInfo = null;
    private static Image topDropPanel;
    private static Image bottomDropPanel;

    private void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag("CommandManager").GetComponent<CommandManager>();
        topDropPanel = GameObject.FindGameObjectWithTag("TopDropPanel").GetComponent<Image>();
        bottomDropPanel = GameObject.FindGameObjectWithTag("BottomDropPanel").GetComponent<Image>();
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        prevPos = transform.position;
        Debug.Log(prevPos);
        Debug.Log(eventData.position);
        transform.position = eventData.position;
        GetComponent<Image>().raycastTarget = false;
        dragInfo = this;

        OnRaycastDropPanel();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //Debug.Log(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragAndDrop.OffRaycastDropPanel();

        Debug.Log(transform.position);
        Debug.Log(eventData.position);

        if (!isDropSuccess)
        {
            transform.position = prevPos;
        }
        else
        {
            transform.position = eventData.position;
        }
        isDragging = false;
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
