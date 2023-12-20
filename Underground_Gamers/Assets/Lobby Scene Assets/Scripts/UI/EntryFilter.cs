using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryFilter : MonoBehaviour
{
    public TMP_InputField EntrySearch;
    public GameObject EntryField;
    public List<Toggle> LevelToggles;
    public List<Toggle> BreakToggles;
    public List<Toggle> TypeToggles;
    public TMP_Dropdown SortStandard;
    public Toggle OrderByToggle;
    public Image filterButton;
    public TMP_Text filterButtonText;

    private StringTable st;
    private void Start()
    {
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }
    
    public void Filtering()
    {
        List<int> dupleBllockCodes = new List<int>();
        foreach (var item in PlayerChanger.instance.usingList)
        {
            if (item.code != -1)
            {
                dupleBllockCodes.Add(item.code);
            }
        }
        if (dupleBllockCodes.Count > 0)
        {
            dupleBllockCodes = dupleBllockCodes.Distinct().ToList();
            dupleBllockCodes.Remove(PlayerChanger.instance.usingList[PlayerChanger.instance.currentSlotIndex].code);
        }


        switch (SortStandard.value)
        {
            case 0:
                GamePlayerInfo.instance.SortPlayersWithLevel(OrderByToggle.isOn);
                break;
            case 1:
                GamePlayerInfo.instance.SortPlayersWithGrade(OrderByToggle.isOn);
                break;
            case 2:
                GamePlayerInfo.instance.SortPlayersWithID(OrderByToggle.isOn);
                break;
        }
        PlayerChanger.instance.StartChange();

        List<Player> players = GamePlayerInfo.instance.havePlayers;
        if (players.Count <= 0)
        {
            return;
        }

        var buttons = PlayerChanger.instance.olds;
        int index = 0;

        foreach (var player in players)
        {
            if (EntrySearch.text != "" &&
            !player.name.Contains(EntrySearch.text))
            {
                buttons[index].gameObject.SetActive(false);
            }
            else
            {
                buttons[index].gameObject.SetActive(CanActiveWithFilter(player));
            }

            if (dupleBllockCodes.Count > 0 && dupleBllockCodes.Contains(player.code))
            {
                buttons[index].SetActive(false);
            }

            index++;
        }
    }

    public bool CanActiveWithFilter(Player player)
    {
        filterButton.color = Color.yellow;
        filterButtonText.text = st.Get("filter");

        //Grade
        bool isOnLevel = true;
        foreach (var LevelToggle in LevelToggles)
        {
            if (LevelToggle.isOn)
            {
                isOnLevel = false;
                break;
            }
        }
        if (!isOnLevel)
        {
            for (int i = 0; i < 3; i++)
            {
                if (LevelToggles[i].isOn && player.grade == i + 3)
                {
                    return true;
                }
            }
        }
        //Break
        bool isOnBreak = true;
        foreach (var BreakToggle in BreakToggles)
        {
            if (BreakToggle.isOn)
            {
                isOnBreak = false;
                break;
            }
        }
        if (!isOnBreak)
        {
            for (int i = 0; i < 4; i++)
            {
                if (BreakToggles[i].isOn && player.breakthrough == i)
                {
                    return true;
                }
            }
        }

        bool isOnType = true;
        foreach (var TypeToggle in TypeToggles)
        {
            if (TypeToggle.isOn)
            {
                isOnType = false;
                break;
            }
        }
        if (!isOnType)
        {
            for (int i = 0; i < 4; i++)
            {
                if (TypeToggles[i].isOn && player.type == i+1)
                {
                    return true;
                }
            }
        }

        if (isOnLevel && isOnBreak && isOnType)
        {
            filterButton.color = Color.white;
            filterButtonText.text = st.Get("all");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetToggleList()
    {
        foreach (var toggles in LevelToggles)
        {
            toggles.isOn = false;
        }
        foreach (var toggles in BreakToggles)
        {
            toggles.isOn = false;
        }
        foreach (var toggles in TypeToggles)
        {
            toggles.isOn = false;
        }
        filterButton.color = Color.white;
        filterButtonText.text = st.Get("all");
    }

    public void ResetSortStandard()
    {
        SortStandard.value = 0;
        OrderByToggle.isOn = true;
    }
}
