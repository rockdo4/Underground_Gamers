using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleUIStory : ScheduleUISubscriber
{
    [SerializeField]
    private int[] minStage = new int[4];
    [SerializeField]
    private Toggle[] stageToggles = new Toggle[4];
    [SerializeField]
    private GameObject[] Panels = new GameObject[4];
    [SerializeField]
    private Image stageTypeImage;
    [SerializeField]
    private StageEnemyInfoImage[] enemyImages = new StageEnemyInfoImage[5];
    [SerializeField]
    private GameObject[] rewardItems = new GameObject[3];

    public GameObject panel;        // ¶ç¿öÁú ÆÐ³Î

    private bool isPanelVisible = false;
    private void Start()
    {
        int count = 0;
        foreach (var item in enemyImages)
        {
            int val = count++;
            item.GetComponent<Button>().onClick.AddListener(() => TogglePanelVisibility(val));
        }
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        lobbyTopMenu.AddFunction(OnBack);
        int lastLevel = GamePlayerInfo.instance.cleardStage;
        for (int i = 0; i < stageToggles.Length; i++)
        {
            bool isAvailable = lastLevel >= minStage[i];
            stageToggles[i].interactable = isAvailable;
            stageToggles[i].isOn = isAvailable;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
    private void OnBack()
    {
        scheduleUIManager.OpenWindow(1);
    }
    public void SetLeague()
    {
        for (int i = 0; i < 4; i++)
        {
            bool isOn = stageToggles[i].isOn;
            Panels[i].SetActive(isOn);
            if (isOn)
            {
                stageToggles[i].GetComponent<ChapterTabs>().SetFirst();
            }
        }
    }

    public void SetStage(int code)
    {
        //stageTypeImage
        //enemyImages
        //rewardItems
        GameInfo.instance.currentStage = code;
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
                RectTransform panelRect = panel.GetComponent<RectTransform>();
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
        panel.SetActive(true);
        var rt = panel.GetComponent<RectTransform>();
        rt.position = enemyImages[index].GetComponent<RectTransform>().position;
        isPanelVisible = true;
    }

    private void HidePanel()
    {
        panel.SetActive(false);
        isPanelVisible = false;
    }
}

