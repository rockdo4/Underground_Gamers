using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIAnimator : MonoBehaviour
{
    public List<MainUIAnimatedElements> animations;
    private bool isStart = false;

    public void DoOpen()
    {
        foreach (var anim in animations)
        {
            anim.OnOpen();
        }
    }
}
