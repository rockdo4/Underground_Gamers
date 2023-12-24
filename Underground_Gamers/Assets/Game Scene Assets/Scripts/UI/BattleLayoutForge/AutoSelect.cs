using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoSelect : MonoBehaviour
{
    public GameManager gameManager;
    private bool isAttackBottom = true;
    private bool isDefendBottom = true;

    public TextMeshProUGUI selectLineButtonText;
    public TextMeshProUGUI autoSelectButtonText;
    public TextMeshProUGUI attackSelectButtonText;
    public TextMeshProUGUI defendSelectButtonText;

    public void Start()
    {
        selectLineButtonText.text = gameManager.str.Get("select line");
        autoSelectButtonText.text = gameManager.str.Get("auto select");
        attackSelectButtonText.text = gameManager.str.Get("bottom attack select");
        defendSelectButtonText.text = gameManager.str.Get("bottom defend select");
    }

    public void SelectAutoAll()
    {
        SelectAuto(0, 2, 3);
        isAttackBottom = true;
        isDefendBottom = true;
        attackSelectButtonText.text = gameManager.str.Get("bottom attack select");
        defendSelectButtonText.text = gameManager.str.Get("bottom defend select");
    }

    // 공격과 수비는 토글 방식
    public void SelectAutoAttack()
    {
        if(isAttackBottom)
        {
            SelectAuto(0, 1, 6);
            attackSelectButtonText.text = gameManager.str.Get("top attack select");
        }
        else
        {
            SelectAuto(1, 2, 6);
            attackSelectButtonText.text = gameManager.str.Get("bottom attack select");
        }

        isAttackBottom = !isAttackBottom;
        defendSelectButtonText.text = gameManager.str.Get("bottom defend select");
        isDefendBottom = true;
    }

    public void SelectAutoDefend()
    {
        if (isDefendBottom)
        {
            SelectAuto(2, 3, 6);
            defendSelectButtonText.text = gameManager.str.Get("top defend select");
        }
        else
        {
            SelectAuto(3, 4, 6);
            defendSelectButtonText.text = gameManager.str.Get("bottom defend select");
        }
        isDefendBottom = !isDefendBottom;
        attackSelectButtonText.text = gameManager.str.Get("bottom attack select");
        isAttackBottom = true;
    }


    /// <summary>
    /// 0 ~ 1 Attack / 2 ~ 3 Defend
    /// </summary>
    public void SelectAuto(int minInclusive, int maxExclusive, int entryCountInclusive)
    {
        AllReleaseEntry();
        foreach (var slot in gameManager.battleLayoutForge.Slots)
        {
            int rand = 0;
            List<AIController> randEntry = null;
            randEntry = GetRandomEntry(minInclusive, maxExclusive, entryCountInclusive, out rand);

            slot.RegistEntry(randEntry);
            SetParentByRandom(slot, rand);
        }


        SortByAINum(gameManager.entryManager.bottomAttackEntry);
        SortByAINum(gameManager.entryManager.topAttackEntry);
        SortByAINum(gameManager.entryManager.bottomDefendEntry);
        SortByAINum(gameManager.entryManager.topDefendEntry);
        gameManager.entryManager.RefreshSelectLineButton();
    }

    public void SetParentByRandom(DragBattleLayoutSlot slot, int rand)
    {
        Transform parent = rand switch
        {
            0 => gameManager.entryManager.bottomAttackEntry,
            1 => gameManager.entryManager.topAttackEntry,
            2 => gameManager.entryManager.bottomDefendEntry,
            3 => gameManager.entryManager.topDefendEntry,
            _ => gameManager.entryManager.bottomAttackEntry
        };

        slot.SetParent(parent);
    }

    public void SortByAINum(Transform parent)
    {
        var childs = parent.GetComponentsInChildren<DragBattleLayoutSlot>();
        System.Array.Sort(childs, CompareByAINum);
        foreach (var child in childs)
        {
            int index = child.AI.aiIndex;
            child.transform.SetSiblingIndex(index);
        }
    }

    private int CompareByAINum(DragBattleLayoutSlot a, DragBattleLayoutSlot b)
    {
        return a.AI.aiIndex.CompareTo(b.AI.aiIndex);
    }

    /// <summary>
    /// 0 ~ 1 Attack / 2 ~ 3 Defend
    /// </summary>
    private List<AIController> GetRandomEntry(int minInclusive, int maxExclusive, int entryCountInclusive, out int rand)
    {
        List<AIController> entry = null;
        do
        {
            rand = Random.Range(minInclusive, maxExclusive);
            entry = rand switch
            {
                0 => gameManager.entryManager.BottomAttackEntryAI,
                1 => gameManager.entryManager.TopAttackEntryAI,
                2 => gameManager.entryManager.BottomDefendEntryAI,
                3 => gameManager.entryManager.TopDefendEntryAI,
                _ => gameManager.entryManager.BottomAttackEntryAI
            };
        } while (entry.Count >= entryCountInclusive);

        return entry;
    }

    private void AllReleaseEntry()
    {
        foreach (var slot in gameManager.battleLayoutForge.Slots)
        {
            slot.ReleaseEntry();
        }
        gameManager.entryManager.BottomAttackEntryAI.Clear();
        gameManager.entryManager.TopAttackEntryAI.Clear();
        gameManager.entryManager.BottomDefendEntryAI.Clear();
        gameManager.entryManager.TopDefendEntryAI.Clear();
        gameManager.entryManager.NoneEntryAI.Clear();
    }
}
