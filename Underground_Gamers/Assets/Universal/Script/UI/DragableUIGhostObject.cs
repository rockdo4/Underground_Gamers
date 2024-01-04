using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableUIGhostObject : DragableUIObject
{
    public GameObject ghostImagePrefab;
    public Image image;
    private GameObject currentGhostObject;
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        currentGhostObject = Instantiate(ghostImagePrefab);
        currentGhostObject.transform.SetParent(transform.parent.parent.parent);
        currentGhostObject.transform.position = eventData.position;
        //Debug.Log(eventData.position);
        var img = currentGhostObject.GetComponent<Image>();
        img.sprite = image.sprite;
        img.color = new Color(1f, 1f, 1f, .5f);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        currentGhostObject.transform.position = eventData.position;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Destroy(currentGhostObject);
        currentGhostObject = null;
    }

}
