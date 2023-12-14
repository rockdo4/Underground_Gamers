using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainUIAniToTarget : MainUIAnimatedElements
{
    public Vector3 direction;
    public Ease easeType;
    public float duration = 1f;

    private bool isInit = false;
    private RectTransform rt;
    private Vector3 originPos;
    private Vector3 startPos;

    private void DoInit()
    {
        rt = GetComponent<RectTransform>();
        originPos = rt.position;
        Vector3 movePos = Vector3.zero;
        movePos.x = originPos.x - (direction.x * rt.localScale.x / 2f);
        movePos.y = originPos.y - (direction.y * rt.localScale.y / 2f);
        movePos.z = originPos.z;
        startPos = movePos;
        isInit = true;
    }
    public override void OnClose()
    {
        if (!isInit)
        {
            DoInit();
        }
        rt.DOMove(startPos, duration).From(originPos).SetEase(easeType);
    }

    public override void OnOpen()
    {
        if (!isInit)
        {
            DoInit();
        }
        rt.DOMove(originPos, duration).From(startPos).SetEase(easeType);
    }
}
