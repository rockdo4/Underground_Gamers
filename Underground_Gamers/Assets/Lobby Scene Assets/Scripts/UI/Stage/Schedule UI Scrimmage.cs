using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleUIScrimmage : ScheduleUISubscriber
{
    [SerializeField]
    private Toggle[] WeekToggles = new Toggle[7];
    [SerializeField]
    private Toggle[] StartToggles = new Toggle[4];
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button[] itemButtons = new Button[4];
    [SerializeField]
    private int[] minLevel = new int[4];
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text rewardTexts;
    [SerializeField]
    private TMP_Text enterCountTexts;
    [SerializeField]
    private GameObject popupItemInfos;
    [SerializeField]
    private Image popupItemInfoImage;
    [SerializeField]
    private TMP_Text[] popupItemInfoText = new TMP_Text[2];
    private int dayOfWeekNumber = 0;
    private StringTable st;

    protected override void Awake()
    {
        base.Awake();
        int count = 0;
        foreach (var week in WeekToggles) 
        {
            int index = count;
            week.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    OpenWeekToggle(index);
                }
            });
            count++;
        }
    }
    public override void OnEnter()
    {
        System.DateTime currentDate = System.DateTime.Now;
        if (System.DateTime.Now.Day > GamePlayerInfo.instance.lastScrimmageTime.Day)
        {
            GamePlayerInfo.instance.lastScrimmageTime = System.DateTime.Today;
            GamePlayerInfo.instance.scrimmageCount = 3;
        }
        dayOfWeekNumber = ((int)currentDate.DayOfWeek + 6) % 7;
        WeekToggles[dayOfWeekNumber].isOn = true;
        enterCountTexts.text = st.Get("daily_scream_count") + $" {GamePlayerInfo.instance.scrimmageCount}/3";

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

    private void OpenWeekToggle(int index)
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        if (index != dayOfWeekNumber || GamePlayerInfo.instance.scrimmageCount <= 0)
        {
            foreach (var item in StartToggles)
            {
                item.interactable = false;
            }
        }
        else
        {
            int currStage = GamePlayerInfo.instance.cleardStage;
            for (int i = 0; i < StartToggles.Length; i++)
            {
                StartToggles[i].interactable = currStage >= minLevel[i];
            } 
        }
        
        switch(index)
        {

        }
    }
}
