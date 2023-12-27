using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private GameObject[] rewardItems = new GameObject[6];

    public GameObject panel;        // ¶ç¿öÁú ÆÐ³Î
    [SerializeField]
    private Image[] panelImage = new Image[3];
    [SerializeField]
    private TMP_Text[] panelText = new TMP_Text[3];

    private bool isPanelVisible = false;
    private StageTable st;
    //private PlayerTable pt;
    //private Vector3 enemySpriteSet = new Vector3(40, 20);

    private List<GameObject> oldPlayerImages = new List<GameObject>();
    private void Start()
    {
        int count = 0;
        foreach (var item in enemyImages)
        {
            int val = count++;
            item.GetComponent<Button>().onClick.AddListener(() => TogglePanelVisibility(val));
        }
        st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        //pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
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
            if (isAvailable)
            {
                stageToggles[i].GetComponentInChildren<TMP_Text>().color = Color.white;
            }
            else
            {
                stageToggles[i].GetComponentInChildren<TMP_Text>().color = Color.gray;
            }
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
        HidePanel();
        if (st == null)
        {
            st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
            //pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        }
        if (oldPlayerImages == null)
        {
            oldPlayerImages = new List<GameObject>();
        }
        foreach (var item in oldPlayerImages)
        {
            Destroy(item);
        }
        oldPlayerImages.Clear();

        StageInfo stageInfo = st.GetStageInfo(code);
        for (int i = 0; i < 6; i++)
        {
            rewardItems[i].SetActive(stageInfo.rewards[i] > 0);
        }
        if (stageInfo.code <= GamePlayerInfo.instance.cleardStage)
        {
            rewardItems[5].SetActive(false);
        }

        var enemysCode = stageInfo.enemys;

        if (enemysCode == null)
        {
            return;
        }

        ////stageTypeImage
        //for (int i = 0; i < enemysCode.Count; i++) 
        //{
        //    var enemyInfo = st.GetEnemyInfo(enemysCode[i]);
        //    var img = Instantiate(st.GetHeadSet(enemysCode[i]), enemyImages[i].images[0].transform);
        //    img.transform.SetParent(enemyImages[i].images[0].transform);
        //    img.transform.localScale = Vector3.one * 130;

        //    SpriteRenderer[] spriteRenderers = img.GetComponentsInChildren<SpriteRenderer>();
        //    foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        //    {
        //        if (spriteRenderer.sprite == null)
        //        {
        //            continue;
        //        }
        //        spriteRenderer.sortingOrder = 0;
        //        //GameObject imageObject = new GameObject(spriteRenderer.name);
        //        //imageObject.transform.SetParent(enemyImages[i].images[0].transform);

        //        //Image image = imageObject.AddComponent<Image>();

        //        //image.sprite = spriteRenderer.sprite;

        //        //RectTransform rectTransform = image.rectTransform;
        //        //rectTransform.position = spriteRenderer.transform.position;
        //        //rectTransform.sizeDelta = new Vector2(spriteRenderer.size.x , spriteRenderer.size.y);

        //        //rectTransform.rotation = spriteRenderer.transform.rotation;

        //        //image.color = spriteRenderer.color;
        //        //oldPlayerImages.Add(imageObject);

        //        //imageObject.layer = LayerMask.NameToLayer("UI");
        //    }
        //    //foreach (var image in oldPlayerImages)
        //    //{
        //    //    if (image.name.Contains("Hair"))
        //    //    {
        //    //        image.transform.SetAsLastSibling();
        //    //    }
        //    //}
        //    //foreach (var image in oldPlayerImages)
        //    //{
        //    //    if (image.name.Contains("Helm"))
        //    //    {
        //    //        image.transform.SetAsLastSibling();
        //    //    }
        //    //}
        //    oldPlayerImages.Add(img);
        //    enemyImages[i].images[1].sprite = pt.playerTypeSprites[enemyInfo.type - 1];

        //}
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
        //panelImage[0].sprite = 
        panelImage[1].sprite = enemyImages[index].images[1].sprite;
        isPanelVisible = true;
        var playerInfo = st.GetEnemyInfo(st.GetStageInfo(GameInfo.instance.currentStage).enemys[index]);
        panelText[0].text = playerInfo.name;
        panelText[1].text = DataTableManager.instance.Get<StringTable>(DataType.String).Get($"type{playerInfo.type}");
    }

    private void HidePanel()
    {
        panel.SetActive(false);
        isPanelVisible = false;
    }
}

