using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraByScale : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 cameraRight;

    private void Awake()
    {
        if (transform.childCount > 1 && transform.GetChild(1).GetComponent<RectTransform>() != null)
        {
            SetPlayer();
        }
    }

    public void SetPlayer()
    {
        rect = transform.GetChild(1).GetComponent<RectTransform>();
        cameraRight = Camera.main.transform.right;
    }
    // Update is called once per frame
    void Update()
    {
        float dot = Vector3.Dot(transform.forward, cameraRight);

        Vector3 newScale = rect.localScale;
        if (dot > 0)
        {           
            newScale.x = -1;
        }
        else
        {
            newScale.x = 1;
        }

        rect.localScale = newScale;
    }
}
