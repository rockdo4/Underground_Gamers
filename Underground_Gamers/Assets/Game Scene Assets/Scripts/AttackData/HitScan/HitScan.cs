using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScan : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float speed;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 dir;
    private Vector3 movePos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetHitScan(Vector3 attacPos, Vector3 hitPos)
    {
        startPos = attacPos;
        endPos = hitPos;
        dir = endPos - startPos;
        dir.Normalize();
        movePos = startPos;
    }

    private void Update()
    {
        Fire();
        DestroyByDistance();
    }

    private void Fire()
    {
        movePos += (dir * speed * Time.deltaTime);
        lineRenderer.SetPosition(0, movePos);
    }

    private void DestroyByDistance()
    {
        float dis = Vector3.Distance(startPos, movePos);
        if(dis < 0.1f)
        {
            gameObject.SetActive(false);
        }
    }
}
