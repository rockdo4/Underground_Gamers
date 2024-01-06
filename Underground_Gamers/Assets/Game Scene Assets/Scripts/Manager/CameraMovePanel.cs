using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Camera mainCamera;
    public float dragSpeed = 0.8f;

    public float minZClamp = -15f;
    public float maxZClamp = 15f;

    private float screenWidth;

    private Vector2 originalPanelPosition;
    private Vector2 dragOrigin;
    void Start()
    {
        originalPanelPosition = transform.position;
        screenWidth = Screen.width;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("드래그중");
        Vector3 deltaPosition = eventData.position - dragOrigin;
        float normalizedDeltaX = deltaPosition.x / screenWidth;
        Vector3 move = new Vector3(0, 0, -normalizedDeltaX) * dragSpeed;
        mainCamera.transform.Translate(move * Time.unscaledTime, Space.World);
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
