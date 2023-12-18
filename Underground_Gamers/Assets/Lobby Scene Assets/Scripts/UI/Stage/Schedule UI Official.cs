using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleUIOfficial : ScheduleUISubscriber
{
    [SerializeField]
    private GameObject UI_OfficialSelect;
    [SerializeField]
    private GameObject UI_OfficialMain;

    private int officialLevel = -1;
    public override void OnEnter()
    {
        base.OnEnter();
        if (GamePlayerInfo.instance.isOnOfficial)
        {
            UI_OfficialSelect.SetActive(false);
            UI_OfficialMain.SetActive(true);
        }
        else
        {
            UI_OfficialSelect.SetActive(true);
            UI_OfficialMain.SetActive(false);
        }
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

    public void SaveOfficialLevel(int level)
    {
        officialLevel = level;
    }

    public void StartOfficial()
    {
        GamePlayerInfo.instance.isOnOfficial = true;
        GamePlayerInfo.instance.officialLevel = officialLevel;
        GamePlayerInfo.instance.officialPlayers = new List<Player>();
        foreach (var player in GamePlayerInfo.instance.usingPlayers)
        {
            GamePlayerInfo.instance.officialPlayers.Add(player);
        }
        GamePlayerInfo.instance.enemyTeams = new List<Player>[7];


        lobbyTopMenu.ExecuteFunction();
        UI_OfficialSelect.SetActive(false);
        UI_OfficialMain.SetActive(true);


    }


}
