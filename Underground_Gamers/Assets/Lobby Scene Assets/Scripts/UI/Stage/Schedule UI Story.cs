using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleUIStory : ScheduleUISubscriber
{
    [SerializeField]
    private List<ChapterTabs> chapterTabs;
    [SerializeField]
    private GameObject PrefabStageToggles;
    [SerializeField]
    private GameObject[] rewardItems = new GameObject[3];
    [SerializeField]
    private Image stageTypeImage;
    [SerializeField]
    private StageEnemyInfoImage[] enemyImages = new StageEnemyInfoImage[5];
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private GameObject popUpEnemyInfo;
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
