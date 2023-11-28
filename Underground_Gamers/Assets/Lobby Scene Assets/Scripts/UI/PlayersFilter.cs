using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayersFilter : MonoBehaviour
{

    public TMP_InputField EntrySearch;
    public GameObject EntryField;
    public List<Toggle> LevelToggles;
    public List<Toggle> BreakToggles;
    public List<Toggle> TypeToggles;
    public TMP_Dropdown SortStandard;
    public Toggle OrderByToggle;

    private StringTable st;
    private void Start()
    {
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }

    public void Filtering()
    {
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
        PlayerChanger.instance.OpenPlayers();

        List<Player> players = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.usingPlayers;
        foreach (var item in usingPlayers)
        {
            if (item.code != -1)
            {
                players.Add(item);
            }
        }
        //int useCount = players.Count;
        players.AddRange(GamePlayerInfo.instance.havePlayers);
        if (players.Count <= 0)
        {
            return;
        }

        var buttons = PlayerChanger.instance.oldsPlayerList;
        int index = 0;

        foreach (var player in players)
        {
            if (EntrySearch.text != "" &&
            !player.name.Contains(EntrySearch.text))
            {
                buttons[index].gameObject.SetActive(false);
                index++;
            }
            else
            {
                buttons[index].gameObject.SetActive(CanActiveWithFilter(player));
                index++;
            }
        }
    }

    public bool CanActiveWithFilter(Player player)
    {
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
                if (TypeToggles[i].isOn && player.type == i + 1)
                {
                    return true;
                }
            }
        }

        return isOnLevel && isOnBreak && isOnType;
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
    }

    public void ResetSortStandard()
    {
        SortStandard.value = 0;
        OrderByToggle.isOn = true;
    }
}
