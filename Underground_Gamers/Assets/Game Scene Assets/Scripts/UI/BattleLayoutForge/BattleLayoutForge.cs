using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLayoutForge : MonoBehaviour
{
    public GameManager gameManager;
    public List<DragBattleLayoutSlot> Slots { get; private set; } = new List<DragBattleLayoutSlot>();
    public SelectLine selectLineButton;

    public TextMeshProUGUI giveUpButtomText;

    private void Start()
    {
        giveUpButtomText.text = gameManager.str.Get("give up");
    }
    public void AddSlot(DragBattleLayoutSlot slot)
    {
        Slots.Add(slot);
    }

    public void ClearSlot()
    {
        foreach(var slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }
        Slots.Clear();
    }


    public void SetActiveSelectLineButton(bool isActive)
    {
        selectLineButton.SetActive(isActive);
    }

    public void SetActiveBattleLayoutForge(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
