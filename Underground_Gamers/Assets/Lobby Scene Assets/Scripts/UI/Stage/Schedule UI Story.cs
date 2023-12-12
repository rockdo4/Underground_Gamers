using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleUIStory : ScheduleUISubscriber
{
    [SerializeField]
    private List<ChapterTabs> chapterTabs;
    [SerializeField]
    private GameObject PrefabStageToggles;
    [SerializeField]
    private Transform toggleTransform;

    private void Start()
    {
        if (chapterTabs != null)
        {
            foreach (var chapterTab in chapterTabs)
            {
                chapterTab.MakeButtons(PrefabStageToggles, toggleTransform, this);
            }
        }
    }
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
    public void SetStage(int code)
    {

    }

    public void ReleaseStage()
    {

    }
}
