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

    public Image[] images;

    public static GameObject dragInfo = null;

    private void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag("CommandManager").GetComponent<CommandManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        isDragging = true;
        prevPos = transform.position;
        transform.position = eventData.position;
        GetComponent<Image>().raycastTarget = false;
        dragInfo = this.gameObject;
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
        isDragging = false;
        transform.position = prevPos;
        dragInfo = null;
    }
}
