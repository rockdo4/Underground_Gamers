using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScheduleType
{
    None,
    Buttons,
    Story,
}

public class ScheduleUIManager : LobbySceneSubscriber
{
    private Dictionary<ScheduleType,ScheduleUISubscriber> scheduleUISubscribers;
    private int currUIIndex = 1;
    public override void OnEnter()
    {
        base.OnEnter();
        LobbyTopMenu lobbyTopMenu = lobbySceneUIManager.lobbyTopMenu;
        lobbyTopMenu.ActiveTop(true);
        lobbyTopMenu.AddFunction(OnBack);
        GamePlayerInfo.instance.isOnSchedule = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(false);
        GamePlayerInfo.instance.isOnSchedule = false;
    }

    private void OnBack()
    {
        lobbySceneUIManager.OpenWindow(1);
    }

    public void Subscribe(ScheduleUISubscriber scheduleUISubscriber, ScheduleType type)
    {
        if (scheduleUISubscribers == null)
        {
            scheduleUISubscribers = new Dictionary<ScheduleType, ScheduleUISubscriber>();
        }
        scheduleUISubscribers.Add(type, scheduleUISubscriber);
    }

    public void OpenWindow(int type)
    {
        scheduleUISubscribers[(ScheduleType)currUIIndex].OnExit();
        currUIIndex = type;
        scheduleUISubscribers[(ScheduleType)type].gameObject.SetActive(true);
        scheduleUISubscribers[(ScheduleType)type].OnEnter();
    }
}
