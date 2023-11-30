using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecruitEffect : MonoBehaviour
{
    public float MaxTime;
    private float startTime;
    private Image image;
    private Color col;
    private void Awake()
    {
        startTime = Time.time;
        image = GetComponent<Image>();
        col = image.color;
    }

    private void Update()
    {
        if (Time.time > startTime + MaxTime)
        {
            Destroy(gameObject);
        }
        col.a = 1 - (Time.time - startTime) / MaxTime;
        image.color = col;
    }
}
