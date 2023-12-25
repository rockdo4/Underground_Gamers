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
    public Image filterButton;
    public TMP_Text filterButtonText;
    [SerializeField]
    private Sprite filterOnSprite;
    [SerializeField]
    private Sprite filterOffSprite;

    private StringTable st;
    private void Start()
    {
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }

    public void Filtering()
    {
        List<Player> list = new List<Player>();
        List<Player> usingPlayers = GamePlayerInfo.instance.usingPlayers;
        foreach (var item in usingPlayers)
        {
            if (item.code != -1)
            {
                list.Add(item);
            }
        }
        switch (SortStandard.value)
        {
            case 0:
                list.AddRange(GamePlayerInfo.instance.CopyOfSortPlayersWithLevel(OrderByToggle.isOn));
                break;
            case 1:
                list.AddRange(GamePlayerInfo.instance.CopyOfSortPlayersWithGrade(OrderByToggle.isOn));
                break;
            default:
                list.AddRange(GamePlayerInfo.instance.CopyOfSortPlayersWithID(OrderByToggle.isOn));
                break;
        }

        List<Player> players = new List<Player>();
        foreach (var item in usingPlayers)
        {
            if (item.code != -1)
            {
                players.Add(item);
            }
        }

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
            !player.name.ToLower().Contains(EntrySearch.text.ToLower()))
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

        foreach (var button in buttons)
        {
            button.transform.SetParent(null);
        }

        foreach (var item in list)
        {
            buttons.Find(b => b.GetComponent<PlayerButtons>().ID == item.ID).transform.SetParent(EntryField.transform);
        }
    }

    public bool CanActiveWithFilter(Player player)
    {
        filterButton.sprite = filterOnSprite;
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
                if (TypeToggles[i].isOn && player.type == i + 1)
                {
                    return true;
                }
            }
        }

        if (isOnLevel && isOnBreak && isOnType)
        {
            filterButton.sprite = filterOffSprite;
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
        filterButton.sprite = filterOffSprite;
        filterButtonText.text = st.Get("all");
    }

    public void ResetSortStandard()
    {
        SortStandard.value = 0;
        OrderByToggle.isOn = true;
    }
}
