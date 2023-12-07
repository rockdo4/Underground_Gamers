using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnDrop : MonoBehaviour, IDropHandler
{
    public CommandManager commandManager;
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        GameObject gameObject = DragAndDrop.dragInfo;
        if (gameObject)
        {
            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<Image>().raycastTarget = true;
            int index = gameObject.GetComponent<CommandInfo>().aiNum - 1;
            commandManager.ExecuteSwitchLine(index);

        }

        var childs = transform.GetComponentsInChildren<CommandInfo>();
        System.Array.Sort(childs, CompareByAINum);

        foreach (var child in childs)
        {
            int index = child.aiNum - 1;
            child.transform.SetSiblingIndex(index);
        }
    }

    private int CompareByAINum(CommandInfo a, CommandInfo b)
    {
        return a.aiNum.CompareTo(b.aiNum);
    }
}


