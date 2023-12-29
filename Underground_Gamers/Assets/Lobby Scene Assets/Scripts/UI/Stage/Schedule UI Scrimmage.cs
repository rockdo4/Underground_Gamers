using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private bool isPanelVisible = false;
    private bool isInit = false;

    public SceneLoader sceneLoader;

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

        count = 0;
        foreach (var StartToggle in StartToggles)
        {
            int level = count;
            StartToggle.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    ScreammageGradeToggle(level);
                }
            });
            count++;
        }
        isInit = true;
    }

    private void Start()
    {
        int count = 0;
        foreach (var item in itemButtons)
        {
            int val = count++;
            item.GetComponent<Button>().onClick.AddListener(() => TogglePanelVisibility(val));
        }
    }
    public override void OnEnter()
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        System.DateTime currentDate = System.DateTime.Now;
        if (System.DateTime.Now.Day > GamePlayerInfo.instance.lastScrimmageTime.Day)
        {
            GamePlayerInfo.instance.lastScrimmageTime = System.DateTime.Today;
            GamePlayerInfo.instance.scrimmageCount = 3;
        }
        dayOfWeekNumber = ((int)currentDate.DayOfWeek + 6) % 7;

        if (!isInit)
        {
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

            count = 0;
            foreach (var StartToggle in StartToggles)
            {
                int level = count;
                StartToggle.onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        ScreammageGradeToggle(level);
                    }
                });
                count++;
            }
            isInit = true;
        }
        WeekToggles[dayOfWeekNumber].isOn = true;
        WeekToggles[dayOfWeekNumber].onValueChanged.Invoke(WeekToggles[dayOfWeekNumber]);
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


        if (index != dayOfWeekNumber || GamePlayerInfo.instance.scrimmageCount <= 0 || GamePlayerInfo.instance.cleardStage < minLevel[0])
        {
            foreach (var item in StartToggles)
            {
                item.interactable = false;
            }
            StartButton.interactable = false;

            int currStage = GamePlayerInfo.instance.cleardStage;
            int currStageGrade = 0;
            for (int i = 0; i < StartToggles.Length; i++)
            {
                bool gradeChecker = currStage >= minLevel[i];
                if (gradeChecker)
                {
                    currStageGrade = i;
                }
                else
                {
                    break;
                }
            }
            ScreammageGradeToggle(currStageGrade);
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
            int currStageGrade = 0;
            for (int i = 0; i < StartToggles.Length; i++)
            {
                bool gradeChecker = currStage >= minLevel[i];
                if (gradeChecker)
                {
                    currStageGrade = i;
                }
                else
                {
                    break;
                }
            }
            ScreammageGradeToggle(currStageGrade);
        }

        switch (index)
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

    private void ScreammageGradeToggle(int screammageLevel)
    {
        GameInfo.instance.screammageLevel = screammageLevel;

        switch (selectedWeekNumber)
        {
            case 0:
            case 2:
            case 4:
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i < screammageLevel + 1)
                        {
                            itemButtons[i].gameObject.SetActive(true);
                        }
                        else
                        {
                            itemButtons[i].gameObject.SetActive(false);
                        }
                    }
                }
                break;
            case 1:
            case 3:
            case 5:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        itemButtons[i].gameObject.SetActive(false);
                    }
                    itemButtons[4].gameObject.SetActive(true);
                }
                break;
            case 6:
                for (int i = 0; i < 4; i++)
                {
                    if (i < screammageLevel + 1)
                    {
                        itemButtons[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        itemButtons[i].gameObject.SetActive(false);
                    }
                    itemButtons[4].gameObject.SetActive(true);
                }
                break;
        }
    }


    private void TogglePanelVisibility(int index)
    {
        if (isPanelVisible)
        {
            HidePanel();
        }
        else
        {
            ShowPanel(index);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (isPanelVisible && touch.phase == TouchPhase.Began)
            {
                RectTransform panelRect = popupItemInfos.GetComponent<RectTransform>();
                Vector2 panelMin = panelRect.position;
                Vector2 panelMax = (Vector2)panelRect.position + panelRect.sizeDelta;

                Vector2 touchPos = touch.position;
                if (touchPos.x < panelMin.x || touchPos.x > panelMax.x ||
                    touchPos.y < panelMin.y || touchPos.y > panelMax.y)
                {
                    HidePanel();

                }

            }
        }

    }
    private void ShowPanel(int index)
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        popupItemInfos.SetActive(true);
        var rt = popupItemInfos.GetComponent<RectTransform>();
        rt.position = itemButtons[index].GetComponent<RectTransform>().position;
        popupItemInfoImage.sprite = itemButtonsSprites[index];
        isPanelVisible = true;
        popupItemInfoText[0].text = st.Get(popupItemInfoTextIds[index]);
        popupItemInfoText[1].text = st.Get("have_mount") + " : " + index switch
        {
            0 or 1 or 2 or 3 => GamePlayerInfo.instance.XpItem[index].ToString(),
            _ => GamePlayerInfo.instance.money.ToString()
        };
    }

    private void HidePanel()
    {
        popupItemInfos.SetActive(false);
        isPanelVisible = false;
    }
    public void StartGame()
    {
        GameInfo.instance.gameType = GameType.Scrimmage;
        GamePlayerInfo.instance.scrimmageCount--;
        //SceneManager.LoadScene("Game Scene");
        sceneLoader.SceneLoad("Game Scene");
    }
}
