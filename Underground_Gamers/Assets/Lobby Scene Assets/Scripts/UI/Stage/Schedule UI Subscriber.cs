using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScheduleUISubscriber : MonoBehaviour
{
    [SerializeField]
    public ScheduleUIManager scheduleUIManager;
    [HideInInspector]
    public LobbyTopMenu lobbyTopMenu;
    [SerializeField]
    public ScheduleType scheduleType;

    protected virtual void Awake()
    {
        scheduleUIManager.Subscribe(this, scheduleType);
        lobbyTopMenu = scheduleUIManager.lobbySceneUIManager.lobbyTopMenu;
    }
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public virtual void OnExit()
    {
        gameObject.SetActive(false);
    }
}
