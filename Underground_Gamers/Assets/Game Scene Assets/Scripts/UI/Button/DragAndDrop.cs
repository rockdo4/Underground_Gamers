using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerDownHandler
{
    private RectTransform canvas;
    private Vector3 prevPos;

    public Vector2 localPointerPosition;

    private CommandManager commandManager;
    public bool isDragging;
    public bool isDropSuccess = false;

    public static DragAndDrop dragInfo = null;
    private static Image topDropPanel;
    private static Image bottomDropPanel;

    public Image originImage;

    public DummySlot dummyPrefab;
    private DummySlot dummy;

    public DummyCommand dummyObject;
    private DummyCommand dummyCommand;

    private void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag("CommandManager").GetComponent<CommandManager>();
        topDropPanel = GameObject.FindGameObjectWithTag("TopDropPanel").GetComponent<Image>();
        bottomDropPanel = GameObject.FindGameObjectWithTag("BottomDropPanel").GetComponent<Image>();
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<RectTransform>();
        dummy = Instantiate(dummyPrefab, canvas);
        dummyCommand = Instantiate(dummyObject, canvas);
        dummy.gameObject.SetActive(false);
        dummyCommand.gameObject.SetActive(false);
        
        dummyCommand.SetDummy(this.GetComponent<CommandInfo>());
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        isDragging = true;
        dummyObject.transform.position = gameObject.transform.position;

        prevPos = transform.position;
        Debug.Log("BeginDrag");
        transform.position = eventData.position;
        GetComponent<Image>().raycastTarget = false;
        dragInfo = this;
        dummy.gameObject.SetActive(true);
        dummy.portrait.sprite = originImage.sprite;

        dummy.transform.position = eventData.position;


        OnRaycastDropPanel();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, eventData.position, eventData.pressEventCamera, out localPointerPosition);
        transform.position = localPointerPosition;
        //transform.position = eventData.position;
        dummy.transform.position = eventData.position;
        Debug.Log(this.GetComponent<CommandInfo>());
        Debug.Log(this.GetComponent<CommandInfo>().skillCoolTime.fillAmount);
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragAndDrop.OffRaycastDropPanel();
        dummy.gameObject.SetActive(false);
        dummyCommand.transform.position = this.transform.position;
        dummyCommand.gameObject.SetActive(false);

        Debug.Log("EndDrag");

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

    public void OnPointerDown(PointerEventData eventData)
    {
        dummyCommand.gameObject.SetActive(true);
        dummyCommand.transform.position = this.transform.position;
        dummyCommand.SetDummy(this.GetComponent<CommandInfo>());
    }
}
