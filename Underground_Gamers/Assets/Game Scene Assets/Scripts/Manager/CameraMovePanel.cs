using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Camera mainCamera;
    public float dragSpeed = 2f;

    public float minZClamp = -15f;
    public float maxZClamp = 15f;

    private Vector2 originalPanelPosition;
    private Vector2 dragOrigin;
    void Start()
    {
        originalPanelPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그중");
        Vector3 deltaPosition = eventData.position - dragOrigin;
        Vector3 move = new Vector3(0, 0, -deltaPosition.x * dragSpeed);
        mainCamera.transform.Translate(move * Time.deltaTime, Space.World);
        Vector3 movePos = mainCamera.transform.position;
        movePos.z = Mathf.Clamp(movePos.z, minZClamp, maxZClamp);
        mainCamera.transform.position = movePos;
        dragOrigin = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragOrigin = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = originalPanelPosition;
    }
}
