using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class OnDrop : MonoBehaviour, IDropHandler
{
    public CommandManager commandManager;
    public GameObject dropPanel;

    private void Start()
    {
        Invoke("LateUpdateSize", 0.1f);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        GameObject gameObject = DragAndDrop.dragInfo;
        if (gameObject && gameObject.transform.parent != dropPanel.transform)
        {
            gameObject.transform.SetParent(dropPanel.transform);
            gameObject.GetComponent<Image>().raycastTarget = true;
            int index = gameObject.GetComponent<CommandInfo>().aiNum - 1;
            commandManager.ExecuteSwitchLine(index);
            // Á¤·Ä
            var childs = dropPanel.transform.GetComponentsInChildren<CommandInfo>();
            System.Array.Sort(childs, CompareByAINum);

            foreach (var child in childs)
            {
                int childIndex = child.aiNum - 1;
                child.transform.SetSiblingIndex(childIndex);
            }

            Invoke("LateUpdateSize", 0.1f);
        }
    }

    private void LateUpdateSize()
    {
        transform.GetComponent<RectTransform>().sizeDelta = dropPanel.GetComponent<RectTransform>().sizeDelta;
    }

    private int CompareByAINum(CommandInfo a, CommandInfo b)
    {
        return a.aiNum.CompareTo(b.aiNum);
    }
}


