using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIAnimator : MonoBehaviour
{
    public List<MainUIAnimatedElements> animations;
    private bool isStart = false;
    void Start()
    {
        Invoke("DoOpen", 0.1f);
    }

    public void DoOpen()
    {
        foreach (var anim in animations)
        {
            anim.OnOpen();
        }
    }
}
