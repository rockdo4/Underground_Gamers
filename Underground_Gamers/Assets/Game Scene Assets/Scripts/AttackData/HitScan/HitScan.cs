using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScan : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public float fadeDuration = 0.3f;
    private float timer;

    public ObjectPool<HitScan> pool;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        timer = Time.time;
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        startColor.a = 1.0f;
        endColor.a = 0.0f;

        lineRenderer.DOColor(new Color2(startColor, startColor), new Color2(endColor, endColor),fadeDuration);
    }

    private void Update()
    {
        if(timer + fadeDuration < Time.time)
            pool.ReturnObjectToPool(this);
    }
}
