using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBattleLayoutSlot : DragSlot
{
    public DragBattleLayoutGhostImage ghostImagePrefab;
    private DragBattleLayoutGhostImage ghostImage;

    public Image illustration;

    private Transform uiCanvas;
    private GameManager gameManager;
    private BattleLayoutForge battleLayoutForge;

    private List<AIController> currentEntry;

    public AIController AI { get; private set; }

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        battleLayoutForge = GameObject.FindGameObjectWithTag("BattleLayoutForge").GetComponent<BattleLayoutForge>();
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Transform>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        List<DragBattleLayoutSlot> slots = battleLayoutForge.GetSlots();
        foreach(DragBattleLayoutSlot slot in slots)
        {
            slot.SetActiveAllRayCast(false);
        }
        SetActiveAllRayCast(false);
        ghostImage.SetActiveGhostImage(true);
        ghostImage.transform.position = eventData.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        ghostImage.transform.position = eventData.position;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        List<DragBattleLayoutSlot> slots = battleLayoutForge.GetSlots();
        foreach (DragBattleLayoutSlot slot in slots)
        {
            slot.SetActiveAllRayCast(true);
        }
        ghostImage.SetActiveGhostImage(false);
    }


    public void SetActiveAllRayCast(bool isActive)
    {
        var images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            image.raycastTarget = isActive;
        }
    } 

    public void MatchAI(AIController ai)
    {
        this.AI = ai;
        ghostImage = Instantiate(ghostImagePrefab, uiCanvas);
        ghostImage.SetGhostImage(AI.status.illustration);
        ghostImage.SetActiveGhostImage(false);
        battleLayoutForge.AddSlot(this);
        RegistEntry(gameManager.entryManager.NoneEntryAI);
    }

    public void MatchPortrait()
    {
        illustration.sprite = AI.status.illustration;
    }

    public void SetParent(Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }

    public List<AIController> GetCurrentEntry()
    {
        return currentEntry;
    }

    public void ReleaseEntry()
    {
        currentEntry.Remove(AI);
    }

    public void RegistEntry(List<AIController> entry)
    {
        currentEntry = entry;
        currentEntry.Add(AI);
    }
}
