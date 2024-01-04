using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragInfoSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private RectTransform canvas;
    private Vector3 prevPos;

    public Vector2 localPointerPosition;

    private CommandManager commandManager;
    public bool isDragging;
    public bool isDropSuccess = false;

    public static DragInfoSlot dragInfo = null;
    private static Image topDropPanel;
    private static Image bottomDropPanel;

    public Image originImage;
    public Image frameImage;

    public DummySlot dummyPortraitPrefab;
    private DummySlot dummyPortrait;

    public DummySlot dummyPrefab;
    private DummySlot dummy;


    private void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag("CommandManager").GetComponent<CommandManager>();
        topDropPanel = GameObject.FindGameObjectWithTag("TopDropPanel").GetComponent<Image>();
        bottomDropPanel = GameObject.FindGameObjectWithTag("BottomDropPanel").GetComponent<Image>();
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<RectTransform>();
        dummyPortrait = Instantiate(dummyPortraitPrefab, canvas);
        dummyPortrait.gameObject.SetActive(false);

        dummy = Instantiate(dummyPrefab, canvas);
        dummy.gameObject.SetActive(false);
        dummy.index = GetComponent<CommandInfo>().aiNum - 1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        isDragging = true;

        prevPos = transform.position;
        //Debug.Log("BeginDrag");
        //transform.position = eventData.position;
        GetComponent<Image>().raycastTarget = false;
        dragInfo = this;
        dummyPortrait.gameObject.SetActive(true);
        dummyPortrait.portrait.sprite = originImage.sprite;
        dummyPortrait.frame.color = frameImage.color;
        dummyPortrait.transform.position = eventData.position;


        OnRaycastDropPanel();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, eventData.position, eventData.pressEventCamera, out localPointerPosition);
        //transform.position = localPointerPosition;
        //transform.position = eventData.position;
        dummyPortrait.transform.position = eventData.position;
        dummy.transform.position = localPointerPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragInfoSlot.OffRaycastDropPanel();
        dummyPortrait.gameObject.SetActive(false);
        dummy.transform.gameObject.SetActive(false);

        //Debug.Log("EndDrag");

        if (!isDropSuccess)
        {
            transform.position = prevPos;
        }
        else
        {
            transform.position = eventData.position;
        }
        isDragging = false;
        dragInfo = null;
    }

    public void OnRaycastDropPanel()
    {
        topDropPanel.raycastTarget = true;
        bottomDropPanel.raycastTarget = true;
    }
    public static void OffRaycastDropPanel()
    {
        topDropPanel.raycastTarget = false;
        bottomDropPanel.raycastTarget = false;
    }
}
