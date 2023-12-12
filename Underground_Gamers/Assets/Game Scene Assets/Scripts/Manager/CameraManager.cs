using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 originPos;
    private float originFOV;

    private float zoomValue;
    public float zoomInValue;

    private bool zoomIn;
    private bool tryZoomIn;   
    private bool zoomOut;
    private bool tryZoomOut;

    private float zoomTimer = 0f;

    public float zoomTime;
    public float followTime;

    private void Awake()
    {
        mainCam = Camera.main;
        originFOV = mainCam.fieldOfView;
        originPos = mainCam.transform.position;
    }

    private void Update()
    {
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
        //mainCam.transform.DOMoveX();
        //zoomTimer += Time.deltaTime;
        //float t = zoomTimer / zoomTime;
        //t= Utils.GetEaseOutQuint(t);
        //float zoomInFov = Mathf.Lerp(zoomValue, zoomInValue, t);
        //mainCam.fieldOfView = zoomInFov;
        //Debug.Log($"IN{t}");

        //if (t > 1) 
        //{
        //    Debug.Log("ZOOMIN_END");
        //    zoomTimer = 0f;
        //    zoomIn = true;
        //    tryZoomIn = false;
        //    mainCam.fieldOfView = zoomInValue;
        //}
    }

    private void ZoomOut()
    {
        mainCam.DOFieldOfView(60, 1f).SetEase(Ease.OutQuint);
        //zoomTimer += Time.deltaTime;
        //float t = zoomTimer / zoomTime;
        //float zoomOutFov = Mathf.Lerp(zoomValue, originFOV, t);
        //mainCam.fieldOfView = zoomOutFov;
        //Debug.Log("Out");
        //Debug.Log(t);

        //if (t >= 1)
        //{
        //    zoomTimer = 0f;
        //    zoomOut = true;
        //    tryZoomOut = false;
        //    mainCam.fieldOfView = originFOV;
        //}
    }

    public void StartZoomIn()
    {
        zoomValue = mainCam.fieldOfView;
        tryZoomIn = true;
        zoomIn = false;
        tryZoomOut = false;
        zoomIn = false;
        zoomTimer = 0f;
    }

    public void StartZoomOut()
    {
        zoomValue = mainCam.fieldOfView;
        tryZoomOut = true;
        zoomOut = false;
        tryZoomIn = false;
        zoomOut = false;
        zoomTimer = 0f;
    }

    public void Move()
    {

    }


}
