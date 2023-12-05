using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlayerInfo : MonoBehaviour
{
    public static GamePlayerInfo instance
    {
        get
        {
            if (gamePlayerInfo == null)
            {
                gamePlayerInfo = FindObjectOfType<GamePlayerInfo>();
            }
            return gamePlayerInfo;
        }
    }

    private static GamePlayerInfo gamePlayerInfo;

    public List<Player> havePlayers = new List<Player>();
    public List<Player> usingPlayers = new List<Player>();

    public List<Gear> haveGears = new List<Gear>();
    public List<Gear> usingGears = new List<Gear>();

    [HideInInspector]
    public int cleardStage = 0;

    [Space(10f)]
    [Header("Resource")]
    public int money = 1000;
    public int crystal = 1000;
    public int contractTicket = 0;
    public int stamina = 0;
    public List<int> XpItem;

    [HideInInspector]
    public int IDcode = 0;
    [HideInInspector]
    public int PresetCode = 0;
    public List<List<float>> Presets;

    private PlayerTable pt;
    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            usingPlayers.Add(new Player());
        }

        Presets = new List<List<float>>();
        for (int i = 0; i < 4; i++)
        {
            XpItem.Add(0);
            Presets.Add(new List<float>());
            for (int j = 0; j < 8; j++)
            {
                Presets[i].Add(-1f);
            }
        }
    }

    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
    }

    public void SortPlayersWithLevel(bool Orderby)
    {
        if (!Orderby)
        {
            var sortedPeople = havePlayers.OrderBy(p => p.level).ThenBy(p => p.grade).ThenBy(p => p.name).ThenBy(p => p.ID);
            havePlayers = sortedPeople.ToList();
        }
        else
        {
            var sortedPeople = havePlayers.OrderByDescending(p => p.level).ThenByDescending(p => p.grade).ThenByDescending(p => p.name).ThenByDescending(p => p.ID);
            havePlayers = sortedPeople.ToList();
        }
    }

    public void SortPlayersWithGrade(bool Orderby)
    {
        if (!Orderby)
        {
            var sortedPeople = havePlayers.OrderBy(p => p.grade).ThenBy(p => p.name).ThenBy(p => p.level).ThenBy(p => p.ID);
            havePlayers = sortedPeople.ToList();
        }
        else
        {
            var sortedPeople = havePlayers.OrderByDescending(p => p.grade).ThenByDescending(p => p.name).ThenByDescending(p => p.level).ThenByDescending(p => p.ID);
            havePlayers = sortedPeople.ToList();
        }
    }

    public void SortPlayersWithID(bool Orderby)
    {
        if (!Orderby) 
        { 
            var sortedPeople = havePlayers.OrderBy(p => p.ID);
            havePlayers = sortedPeople.ToList();
        }
        else
        {
            var sortedPeople = havePlayers.OrderByDescending(p => p.ID);
            havePlayers = sortedPeople.ToList();
        }
        
    }

    public Player AddPlayer(int code)
    {
        Player newPlayer = new Player();
        if (pt == null)
        {
            pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        }
        PlayerInfo info = pt.playerDatabase[code];
        newPlayer.name = info.name;
        newPlayer.code = code;
        newPlayer.type = info.type;
        newPlayer.grade = info.grade;
        newPlayer.ID = IDPrinter();
        newPlayer.cost = info.cost;
        havePlayers.Add(newPlayer);
        return newPlayer;
    }

    public void RemoveUsePlayer(int slotIndex)
    {
        if (slotIndex > 7)
        {
            return;
        }
        Player newPlayer = new Player();
        usingPlayers[slotIndex] = newPlayer;
    }

    private float IDPrinter()
    {
        if (IDcode == int.MaxValue)
        {
            IDcode = 0;
        }
        float ID = 0.0000001f * IDcode;
        IDcode++;
        return ID;
    }

    public void LoadPreset()
    {
        LoadPreset(PresetCode);
    }
    public void LoadPreset(int index)
    {
        if (index > 4)
        {
            return;
        }

        List<float> newPreset = new List<float>();
        foreach (var item in usingPlayers)
        {
            newPreset.Add(item.ID);
        }
        Presets[PresetCode] = newPreset;

        if (PresetCode == index)
        {
            return;
        }

        PresetCode = index;
        List<float> presetCodeList = Presets[PresetCode];

        for (int i = 0; i < usingPlayers.Count; i++)
        {
            if (usingPlayers[i].code != -1)
            {
                havePlayers.Add(usingPlayers[i]);
                RemoveUsePlayer(i);
            }
        }

        int count = 0;
        List<Player> deletePlayer = new List<Player>();
        foreach (float presetCode in presetCodeList) 
        {
            if (presetCode != -1)
            {
                foreach (var havePlayer in havePlayers)
                {
                    if (havePlayer.ID == presetCode)
                    {
                        usingPlayers[count] = havePlayer;
                        deletePlayer.Add(havePlayer);
                        continue;
                    }
                }
            }
            count++;
        }

        foreach (var item in deletePlayer)
        {
            havePlayers.Remove(item);
        }
    }

    public bool UseMoney(int money, int crystal, int contractTicket)
    {
        if (this.money < money ||
            this.crystal < crystal ||
            this.contractTicket < contractTicket)
        {
            return false;
        }
        this.money -= money;
        this.crystal -= crystal;
        this.contractTicket -= contractTicket;
        return true;
    }

    public bool CheckMoney(int money, int crystal, int contractTicket)
    {
        if (this.money < money ||
            this.crystal < crystal ||
            this.contractTicket < contractTicket)
        {
            return false;
        }
        return true;
    }


    public List<Player> CopyOfSortPlayersWithLevel(bool Orderby)
    {
        if (!Orderby)
        {
            var sortedPeople = havePlayers.OrderBy(p => p.level).ThenBy(p => p.grade).ThenBy(p => p.name).ThenBy(p => p.ID);
            return sortedPeople.ToList();
        }
        else
        {
            var sortedPeople = havePlayers.OrderByDescending(p => p.level).ThenByDescending(p => p.grade).ThenByDescending(p => p.name).ThenByDescending(p => p.ID);
            return sortedPeople.ToList();
        }
    }

    public List<Player> CopyOfSortPlayersWithGrade(bool Orderby)
    {
        if (!Orderby)
        {
            var sortedPeople = havePlayers.OrderBy(p => p.grade).ThenBy(p => p.name).ThenBy(p => p.level).ThenBy(p => p.ID);
            return sortedPeople.ToList();
        }
        else
        {
            var sortedPeople = havePlayers.OrderByDescending(p => p.grade).ThenByDescending(p => p.name).ThenByDescending(p => p.level).ThenByDescending(p => p.ID);
            return sortedPeople.ToList();
        }
    }

    public List<Player> CopyOfSortPlayersWithID(bool Orderby)
    {
        if (!Orderby)
        {
            var sortedPeople = havePlayers.OrderBy(p => p.ID);
            return sortedPeople.ToList();
        }
        else
        {
            var sortedPeople = havePlayers.OrderByDescending(p => p.ID);
            return sortedPeople.ToList();
        }

    }

    public List<Player> GetUsingPlayers()
    {
        List<Player> list = new List<Player>();
        foreach (var item in havePlayers)
        {
            if (item.code >= 0)
            {
                list.Add(item);
            }
        }
        return list;
    }
}
