using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class CameraManager : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 originPos;
    private float originFOV;

    private Vector3 currentPos;

    private float zoomValue;
    public float zoomInValue;

    private bool zoomIn;
    private bool tryZoomIn;   
    private bool zoomOut;
    private bool tryZoomOut;

    private float zoomTimer = 0f;
    private float moveTimer = 0f;

    public float zoomTime;
    public float moveTime;

    public GameManager gameManager;

    private void Awake()
    {
        mainCam = Camera.main;
        originFOV = mainCam.fieldOfView;
        originPos = mainCam.transform.position;
    }

    private void LateUpdate()
    {
        if(tryZoomIn)
        {
            Vector3 movePos = gameManager.commandManager.currentAI.transform.position;
            FollowMove(movePos.z);
        }

        if(!tryZoomIn && tryZoomOut)
        {
            ReturnMove();
        }
        if(tryZoomIn && !zoomIn)
        {
            ZoomIn();
        }

        if(tryZoomOut && !zoomOut)
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        mainCam.DOFieldOfView(40, 1f).SetEase(Ease.OutQuint);
    }

    private void ZoomOut()
    {
        mainCam.DOFieldOfView(60, 1f).SetEase(Ease.OutQuint);
    }

    public void StartZoomIn()
    {
        zoomValue = mainCam.fieldOfView;
        tryZoomIn = true;
        zoomIn = false;
        tryZoomOut = false;
        zoomIn = false;
        moveTimer = 0f;
    }

    public void StartZoomOut()
    {
        currentPos = mainCam.transform.position;
        zoomValue = mainCam.fieldOfView;

        tryZoomIn = false;

        tryZoomOut = true;
        zoomOut = false;
        tryZoomIn = false;
        zoomOut = false;
        moveTimer = 0f;
    }

    public void FollowMove(float zPos)
    {
        moveTimer += Time.deltaTime;
        float t = moveTimer / moveTime;
        t = Utils.GetEaseOutQuintReversed(t);
        currentPos = mainCam.transform.position;
        currentPos.z = zPos;
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, currentPos, t);
    }

    public void ReturnMove()
    {
        moveTimer += Time.deltaTime;
        float t = moveTimer / moveTime;
        t = Utils.GetEaseOutQuintReversed(t);
        Vector3 zoomOutPos = Vector3.Lerp(currentPos, originPos, t);
        mainCam.transform.position = zoomOutPos;

        if (t >= 1)
        {
            moveTimer = 0f;
            zoomOut = true;
            tryZoomOut = false;
            mainCam.transform.position = originPos;
        }
    }


}
