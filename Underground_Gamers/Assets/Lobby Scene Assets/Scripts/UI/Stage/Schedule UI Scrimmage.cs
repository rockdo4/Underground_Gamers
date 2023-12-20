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
    private Button[] itemButtons = new Button[5];
    [SerializeField]
    private Sprite[] itemButtonsSprites = new Sprite[5];
    [SerializeField]
    private int[] minLevel = new int[4];
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite[] imageSprite = new Sprite[3];
    [SerializeField]
    private TMP_Text rewardTexts;
    [SerializeField]
    private string[] rewardTextsIds = new string[3];
    [SerializeField]
    private TMP_Text enterCountTexts;
    [SerializeField]
    private GameObject popupItemInfos;
    [SerializeField]
    private Image popupItemInfoImage;
    [SerializeField]
    private TMP_Text[] popupItemInfoText = new TMP_Text[2];
    [SerializeField]
    private string[] popupItemInfoTextIds = new string[5];
    private int dayOfWeekNumber = 0;
    private int selectedWeekNumber = 0;
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
        selectedWeekNumber = index;


        if (index != dayOfWeekNumber || GamePlayerInfo.instance.scrimmageCount <= 0)
        {
            foreach (var item in StartToggles)
            {
                item.interactable = false;
            }
            StartButton.interactable = false;
        }
        else
        {
            int currStage = GamePlayerInfo.instance.cleardStage;
            for (int i = 0; i < StartToggles.Length; i++)
            {
                bool isInteractable = currStage >= minLevel[i];
                StartToggles[i].interactable = isInteractable;
                if (isInteractable)
                {
                    StartToggles[i].isOn = true;
                }
            }
            StartButton.interactable = true;
        }
        
        switch(index)
        {
            case 0:
            case 2:
            case 4:
                image.sprite = imageSprite[0];
                rewardTexts.text = st.Get(rewardTextsIds[0]);
                break;
            case 1:
            case 3:
            case 5:
                image.sprite = imageSprite[1];
                rewardTexts.text = st.Get(rewardTextsIds[1]);
                break;
            case 6:
                image.sprite = imageSprite[2];
                rewardTexts.text = st.Get(rewardTextsIds[2]);
                break;
        }
    }

    //selectedWeekNumber에 맞춰 오른토글 분기 -> 아랫버튼 표기보이기
    //아랫 버튼눌렀을떄 1-5에 맞춰 뜨는팝업 정보변환 및 다른데누를시 ㅇㅇ
    //스타트 누르고 엔트리로 바뀌는 부분
}
