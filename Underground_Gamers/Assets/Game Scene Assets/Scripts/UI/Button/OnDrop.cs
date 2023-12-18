using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class OnDrop : MonoBehaviour, IDropHandler
{
    public CommandManager commandManager;
    public GameObject dropPanel;
    private Transform prevParent;
    private Transform prevDropPanel;

    private void Start()
    {
        Invoke("LateUpdateSize", 1f);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        Debug.Log(eventData.pointerDrag.gameObject.name);
        DragAndDrop dragObject = DragAndDrop.dragInfo;

        // 이전 기능 드롭 패널과, 실제 이미지 패널 크기 동기화
        prevParent = dragObject.transform.parent.parent.GetChild(1);
        prevDropPanel = dragObject.transform.parent;

        if (dragObject && dragObject.transform.parent != dropPanel.transform)
        {
            dragObject.isDropSuccess = true;
            dragObject.transform.SetParent(dropPanel.transform);
            dragObject.GetComponent<Image>().raycastTarget = true;
            AIController ai = dragObject.GetComponent<CommandInfo>().aiController;
            commandManager.ExecuteSwitchLine(ai);
            // 정렬
            var childs = dropPanel.transform.GetComponentsInChildren<CommandInfo>();
            System.Array.Sort(childs, CompareByAINum);

            foreach (var child in childs)
            {
                int childIndex = child.aiNum - 1;
                child.transform.SetSiblingIndex(childIndex);
            }

            Invoke("LateUpdateSize", 0.1f);
        }
        dragObject.isDropSuccess = false;
    }

    private void LateUpdateSize()
    {
        if (prevParent != null && prevDropPanel != null)
            prevParent.GetComponent<RectTransform>().sizeDelta = prevDropPanel.GetComponent<RectTransform>().sizeDelta;
        transform.GetComponent<RectTransform>().sizeDelta = dropPanel.GetComponent<RectTransform>().sizeDelta;
    }

    private int CompareByAINum(CommandInfo a, CommandInfo b)
    {
        return a.aiNum.CompareTo(b.aiNum);
    }
}


