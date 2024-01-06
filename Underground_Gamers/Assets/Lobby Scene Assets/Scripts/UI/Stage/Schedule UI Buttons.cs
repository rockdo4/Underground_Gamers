using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleUIButtons : ScheduleUISubscriber
{
    [SerializeField]
    private Button[] buttons = new Button[2];
    public override void OnEnter()
    {
        base.OnEnter();
        if (GamePlayerInfo.instance.cleardStage < 106)
        {
            foreach (var item in buttons)
            {
                item.interactable = false;
            }
        }
        else
        {
            foreach (var item in buttons)
            {
                item.interactable = true;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
