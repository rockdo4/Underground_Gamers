using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableUIObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector]
    public static Vector2 originPos;
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        originPos = transform.position;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originPos;
    }

}
