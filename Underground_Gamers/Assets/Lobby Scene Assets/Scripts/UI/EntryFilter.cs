using System.Collections;
using System.Collections.Generic;
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

    private StringTable st;
    private void Start()
    {
        st = DataTableManager.instance.Get<StringTable>(DataType.String);
    }

    public void EntryFiltering(string name)
    {
        List<Player> players = GamePlayerInfo.instance.havePlayers;
        if (players.Count <= 0)
        {
            return;
        }

        var buttons = EntryField.GetComponentsInChildren<PlayerButtons>(true);
        int index = 0;
        foreach ( PlayerButtons button in buttons ) 
        {
            button.gameObject.SetActive(true);
        }

        foreach (var player in players)
        {
            if (EntrySearch.text != "" &&
                !st.Get($"playerName{player.code}").Contains(EntrySearch.text))
            {
                buttons[index].gameObject.SetActive(false);
                index++;
            }
            else if (true)
            {

            }

            index++;
        }

    }
}
