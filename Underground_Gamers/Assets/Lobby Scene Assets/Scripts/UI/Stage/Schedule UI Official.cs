using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleUIOfficial : ScheduleUISubscriber
{
    public override void OnEnter()
    {
        base.OnEnter();
        lobbyTopMenu.AddFunction(OnBack);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
    private void OnBack()
    {
        scheduleUIManager.OpenWindow(1);
    }
}
