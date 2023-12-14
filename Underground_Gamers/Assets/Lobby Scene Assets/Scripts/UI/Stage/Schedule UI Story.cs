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
    private StageTable st;
    private PlayerTable pt;
    private Vector3 enemySpriteSet = new Vector3(40, 20);

    private List<GameObject> oldPlayerImages = new List<GameObject>();
    private GameObject a;
    private void Start()
    {
        int count = 0;
        foreach (var item in enemyImages)
        {
            int val = count++;
            item.GetComponent<Button>().onClick.AddListener(() => TogglePanelVisibility(val));
        }
        st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
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
        if (st == null)
        {
            st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
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
        var enemysCode = st.GetStageInfo(code).enemys;

        if (enemysCode == null)
        {
            return;
        }
        //Debug.Log(enemysCode.Count);
        //stageTypeImage
        for (int i = 0; i < enemysCode.Count; i++) 
        {
            var enemyInfo = st.GetEnemyInfo(enemysCode[i]);
            var img = Instantiate(st.GetHeadSet(enemysCode[i]), enemyImages[i].images[0].transform);


            SpriteRenderer[] spriteRenderers = img.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer.sprite == null)
                {
                    continue;
                }
                GameObject imageObject = new GameObject(spriteRenderer.name);
                imageObject.transform.SetParent(enemyImages[i].images[0].transform);

                Image image = imageObject.AddComponent<Image>();

                image.sprite = spriteRenderer.sprite;

                RectTransform rectTransform = image.rectTransform;
                rectTransform.position = spriteRenderer.transform.position;
                rectTransform.sizeDelta = new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
                rectTransform.rotation = spriteRenderer.transform.rotation;

                image.color = spriteRenderer.color;
                oldPlayerImages.Add(imageObject);

                imageObject.layer = LayerMask.NameToLayer("UI");
            }
            foreach (var image in oldPlayerImages)
            {
                if (image.name.Contains("Hair"))
                {
                    image.transform.SetAsLastSibling();
                }
            }
            foreach (var image in oldPlayerImages)
            {
                if (image.name.Contains("Helm"))
                {
                    image.transform.SetAsLastSibling();
                }
            }
            Destroy(img);
            enemyImages[i].images[1].sprite = pt.playerTypeSprites[enemyInfo.type - 1];
            
        }
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

