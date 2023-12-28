using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutorEffect : MonoBehaviour
{
    public AIController controller;

    public Collider col;
    private float delay;
    private float onColtime;
    private float timer;
    private bool isStart = false;
    private int count;

    private void OnDisable()
    {
        col.enabled = false;
        isStart = false;
    }

    private void Update()
    {
        if(onColtime + timer < Time.time && !isStart)
        {
            isStart = true;
            col.enabled = true;
            timer = Time.time;
        }

        if(isStart)
        {
            if (delay + timer < Time.time && isStart && count < 2)
            {
                count++;
                timer = Time.time;
                col.enabled = true;
                // bool
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        TeamIdentifier identity = other.GetComponent<TeamIdentifier>();
        if (identity == null)
            return;
        if (other.gameObject.layer == controller.gameObject.layer)
            return;

    }

    public void SetEffect(AIController ai, float delay, float onColTime, float timer)
    {
        this.controller = ai;
        this.delay = delay;
        this.onColtime = onColTime;
        this.timer = timer;
    }

    public void SetOffsetNScale(float offset, float scale)
    {
        Vector3 offsetPos = transform.position;
        offsetPos.y += offset;
        transform.position = offsetPos;

        transform.localScale *= scale;
    }
}
