using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Vector3 prevPos;
    private CommandManager commandManager;
    public bool isDragging;

    private void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag("CommandManager").GetComponent<CommandManager>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        prevPos = transform.position;
        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //transform.position = prevPos;
        // 드롭된 위치의 UI 요소 정보 가져오기
        GameObject droppedObject = eventData.pointerDrag;
        GameObject droppedOnObject = eventData.pointerCurrentRaycast.gameObject;

        // 드롭된 위치에 있는 UI 요소가 패널인지 확인
        if (droppedOnObject != null && droppedOnObject.GetComponent<RectTransform>() != null)
        {
            // 해당 UI 요소가 패널이라면 처리할 내용 추가
            Debug.Log($"Drop : {droppedObject.name}");
            Debug.Log($"Drop On : {droppedOnObject.name}");
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            // 패널에 대한 추가 동작 수행
        }
        else
        {
            Debug.Log("드롭된 위치는 패널이 아닙니다.");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        transform.position = prevPos;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        //prevPos = transform.position;
        //transform.position = eventData.position;
    }
}
