using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public int representativePlayer = -1;
    public List<Player> havePlayers = new List<Player>();
    public List<Player> usingPlayers = new List<Player>();
    public List<float> officialPlayers = new List<float>();

    public int cleardStage = 0;
    public string playername = "이감독";

    [Space(10f)]
    [Header("Resource")]
    public int money = 10000;
    public int crystal = 4000;
    public int contractTicket = 0;
    public int stamina = 0;
    public List<int> XpItem;

    [HideInInspector]
    public int IDcode = 0;
    [HideInInspector]
    public int PresetCode = 0;
    public List<List<float>> Presets;

    public List<int> tradeCenter = new List<int>();
    public DateTime lastRecruitTime = DateTime.MinValue;

    public bool isInit = false;

    public string teamName = "드림팀";

    public int playSpeed = 1;

    //정규전
    public bool isOnOfficial = false;
    public int officialLevel = -1;
    public List<EnemyInfo>[] enemyTeams = new List<EnemyInfo>[7];
    public OfficialTeamData[] officialTeamDatas = new OfficialTeamData[8];
    public OfficialPlayerData[] officialPlayerDatas = new OfficialPlayerData[8];
    public int officialWeekNum = 0;
    public int[,] officialMatchResult = new int[7, 8];
    public string[,] officialFinalMatchResultName = new string[3, 2];
    public int[,] officialFinalMatchResult = new int[3, 2];
    public bool endScrimmage = false;

    //스크리밍
    public DateTime lastScrimmageTime = DateTime.MinValue;
    public int scrimmageCount = 3;

    //기타
    public bool isOnSchedule = false;
    private PlayerTable pt;
    private StageTable st;
    private StringTable str;
    public int willOpenMenu = 0;

    [HideInInspector]
    public List<Sprite> itemSpriteList = new List<Sprite>();
    public int[,] officialMatchInfo = new int[7, 8]
        {
            {0,1,2,3,4,5,6,7},
            {1,3,0,5,2,7,4,6},
            {6,1,7,4,5,2,3,0},
            {2,1,4,0,6,3,7,5},
            {1,7,5,6,3,4,0,2},
            {1,4,6,2,7,0,5,3},
            {5,1,3,7,0,6,2,4},
        };
    public int[] officialPlayerMatchInfo = new int[7]
       {6,2,4,5,1,0,3};
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
        for (int i = 0; i < 5; i++)
        {
            tradeCenter.Add(1);
        }
        for (int i = 0; i < 3; i++)
        {
            itemSpriteList.Add(Resources.Load<Sprite>(
              Path.Combine("Items", i.ToString())));
        }

        LoadFile();
    }

    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
        st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        str = DataTableManager.instance.Get<StringTable>(DataType.String);
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
        SaveFile();
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
        SaveFile();
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
        SaveFile();
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
        SaveFile();
    }
    public bool AddMoney(int money, int crystal, int contractTicket)
    {
        if (this.money + money > 999999999||
            this.crystal + crystal > 999999999 ||
            this.contractTicket + contractTicket > 99999)
        {
            this.money = Mathf.Min(this.money+money, 999999999);
            this.money = Mathf.Min(this.crystal + crystal, 999999999);
            this.money = Mathf.Min(this.contractTicket + contractTicket, 99999);
            SaveFile();
            return false;
        }
        this.money += money;
        this.crystal += crystal;
        this.contractTicket += contractTicket;
        SaveFile();
        return true;
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
        SaveFile();
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
        foreach (var item in usingPlayers)
        {
            if (item.code >= 0)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public void AnalyzePlayer(Player player, int level, float xp, float maxXp)
    {
        foreach (var item in usingPlayers) 
        {
            if (item.code != -1 && item.ID == player.ID)
            {
                item.level = level;
                item.xp = xp;
                item.maxXp = maxXp;
                SaveFile();
                return;
            }
        }
        foreach (var item in havePlayers)
        {
            if (item.ID == player.ID)
            {
                item.level = level;
                item.xp = xp;
                item.maxXp = maxXp;
                SaveFile();
                return;
            }
        }
        Debug.Log("Can't find Char");
    }

    public bool GetXpItems(int one,int two, int three, int four)
    {

        if (XpItem[0] + one > 9999 ||
            XpItem[1] + two > 9999 ||
            XpItem[2] + three > 9999 ||
            XpItem[3] + four > 9999)
        {
            XpItem[0] = Math.Min(XpItem[0] + one , 9999);
            XpItem[1] = Math.Min(XpItem[1] + two, 9999);
            XpItem[2] = Math.Min(XpItem[2] + three, 9999);
            XpItem[3] = Math.Min(XpItem[3] + four, 9999);
            SaveFile();
            return false;
        }
        XpItem[0] += one;
        XpItem[1] += two;
        XpItem[2] += three;
        XpItem[3] += four;
        SaveFile();
        return true;
    }

    public void TrainPlayer(Player player, List<int>train,int potential)
    {
        foreach (var item in usingPlayers)
        {
            if (item.code != -1 && item.ID == player.ID)
            {
                item.training.AddRange(train);
                item.potential = potential;
                SaveFile();
                return;
            }
        }
        foreach (var item in havePlayers)
        {
            if (item.ID == player.ID)
            {
                item.training.AddRange(train);
                item.potential = potential;
                SaveFile();
                return;
            }
        }
        Debug.Log("Can't find Char");
    }

    public void BreakPlayer(Player player, List<Player> delete)
    {
        foreach (var item in delete)
        {
            if (usingPlayers.Contains(item))
            {
                RemoveUsePlayer(usingPlayers.IndexOf(item));
            }
            else if (havePlayers.Contains(item))
            {
                havePlayers.Remove(item);
            }
        }

        player.breakthrough++;
        player.maxLevel += 5;
        player.skillLevel = player.breakthrough switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            _ => 1,
        };
        player.potential += 10;
        SaveFile();
    }

    public void SetTradeCenter(List<int> setValue)
    {
        tradeCenter = setValue;
        SaveFile();
    }

    public void CheckRepresentPlayers()
    {
        List<Player> playerList = new List<Player>();
        playerList.AddRange(GetUsingPlayers());
        playerList.AddRange(havePlayers);
        List<int> distinctCodeList = playerList
  .Select(item => item.code)
  .Distinct()
  .ToList();

        if (!distinctCodeList.Contains(representativePlayer))
        {
            representativePlayer = -1;
        }
    }

    public void SaveFile()
    {
        var saveData = new SaveData();

        saveData.representativePlayer = representativePlayer;
        saveData.havePlayers = havePlayers;
        saveData.usingPlayers = usingPlayers;
        if (usingPlayers.Count > 8)
        {
            Debug.Log($"{usingPlayers.Count}");
        }
        
        saveData.cleardStage = cleardStage;
        saveData.playername = playername;
        saveData.money  = money;
        saveData.crystal = crystal;
        saveData.contractTicket = contractTicket;
        saveData.stamina = stamina;
        saveData.XpItem = XpItem;
        saveData.IDcode = IDcode;
        saveData.PresetCode = PresetCode;
        saveData.tradeCenter = tradeCenter;
        saveData.lastRecruitTime = lastRecruitTime;
        saveData.Presets = Presets;

        saveData.officialPlayers = officialPlayers;
        saveData.teamName = teamName;
        saveData.playSpeed = playSpeed;

        saveData.isOnOfficial = isOnOfficial;
        saveData.officialLevel = officialLevel;
        saveData.enemyTeams = enemyTeams;
        saveData.officialTeamDatas = officialTeamDatas;
        saveData.officialPlayerDatas = officialPlayerDatas;
        saveData.officialWeekNum = officialWeekNum;
        saveData.officialMatchResult = officialMatchResult;
        saveData.officialFinalMatchResult = officialFinalMatchResult;
        saveData.officialFinalMatchResultName = officialFinalMatchResultName;
        saveData.endScrimmage = endScrimmage;

        saveData.lastScrimmageTime = lastScrimmageTime;
        saveData.scrimmageCount = scrimmageCount;

        var path = Path.Combine(Application.persistentDataPath, "savefile_v2.json");
        Debug.Log(path);
        var json = JsonConvert.SerializeObject(saveData,new PlayerConverter(), new EnemyInfoConverter(), new OfficialTeamDataConverter(), new OfficialPlayerDataConverter());
        File.WriteAllText(path, json);
    }

    public void LoadFile()
    {
        var path = Path.Combine(Application.persistentDataPath, "savefile_v2.json");
        if (!File.Exists(path))
        {
            isInit = true;
            return;
        }

        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, new PlayerConverter(),new EnemyInfoConverter(), new OfficialTeamDataConverter(), new OfficialPlayerDataConverter());

        representativePlayer = saveData.representativePlayer;
        havePlayers = saveData.havePlayers;

        int count = 0;
        foreach (var item in saveData.usingPlayers)
        {
            usingPlayers[count] = saveData.usingPlayers[count];
            count++;
        }

        cleardStage = saveData.cleardStage;
        playername = saveData.playername;
        money = saveData.money;
        crystal = saveData.crystal;
        contractTicket  = saveData.contractTicket;
        stamina = saveData.stamina;
        XpItem = saveData.XpItem;
        IDcode = saveData.IDcode;
        PresetCode = saveData.PresetCode;
        tradeCenter = saveData.tradeCenter;
        lastRecruitTime = saveData.lastRecruitTime;
        Presets = saveData.Presets;

        officialPlayers = saveData.officialPlayers;
        teamName = saveData.teamName;
        playSpeed = saveData.playSpeed;

        isOnOfficial = saveData.isOnOfficial;
        officialLevel = saveData.officialLevel;
        enemyTeams = saveData.enemyTeams;
        officialTeamDatas = saveData.officialTeamDatas;
        officialPlayerDatas = saveData.officialPlayerDatas;
        officialWeekNum = saveData.officialWeekNum;
        officialMatchResult = saveData.officialMatchResult;
        officialFinalMatchResult = saveData.officialFinalMatchResult;
        officialFinalMatchResultName = saveData.officialFinalMatchResultName;
        endScrimmage = saveData.endScrimmage;

        lastScrimmageTime = saveData.lastScrimmageTime;
        scrimmageCount = saveData.scrimmageCount;
    }

    public void OfficialMakeEnemyTeams(int officialLevel)
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
            str = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        //적 테이블 받아서 생성
        enemyTeams = new List<EnemyInfo>[7];
        for (int i = 0; i < 7; i++)
        {
            enemyTeams[i] = st.MakeOfficialEnemies(officialLevel);
        }

        officialTeamDatas = new OfficialTeamData[8];
        for (int i = 0; i < 7; i++)
        {
            officialTeamDatas[i].name = str.Get($"random_team_name{UnityEngine.Random.Range(0,99)}");
            officialTeamDatas[i].isPlayer = false;
            officialTeamDatas[i].index = i;
        }

        officialTeamDatas[7].name = teamName;
        officialTeamDatas[7].isPlayer = true;
        officialTeamDatas[7].index = 7;

        officialPlayerDatas = new OfficialPlayerData[8];
        officialWeekNum = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                officialMatchResult[i, j] = 0;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                officialFinalMatchResultName[i, j] = "";
                officialFinalMatchResult[i, j] = 0;
            }
        }



        isOnOfficial = true;
        this.officialLevel = officialLevel;
        officialPlayers = new List<float>();
        int count = 0;
        foreach (var player in usingPlayers)
        {
            officialPlayers.Add(player.ID);
            count++;
        }
    }

    public OfficialTeamData[] OfficialTeamRankSort()
    {
        return officialTeamDatas.OrderByDescending(a => a.setWin - a.setLose).ToArray();
    }

    public Player GetOfficialPlayer(int index)
    {
        foreach (var player in usingPlayers)
        {
            if (player.ID == officialPlayers[index])
            {
                return player;
            }
        }
        foreach (var player in havePlayers)
        {
            if (player.ID == officialPlayers[index])
            {
                return player;
            }
        }
        return new Player();
    }

    public void CalculateOfficialPlayer(bool isWin, int setWin, int setLose)
    {
        endScrimmage = false;
        if (officialWeekNum < 7)
        {
            CalculateOfficialPlayerResult(isWin, setWin, setLose);
        }
        else if(officialWeekNum < 9)
        {
            CalculateOfficialFinalsPlayerResult(setWin, setLose);
        }
        UpdateOfficial(isWin);
    }


    //플레이어 정보 업데이트후 ai정보 업데이트
    public void UpdateOfficial(bool isWin)
    {
        //플레이어 경기결과는 받고난뒤여야함, CalculateOfficialPlayerResult()
        officialWeekNum++;
        if (officialWeekNum < 7)
        {
            CalculateOfficialRandomResults();
            //officialPlayerDatas 정보 업데이트
        }
        else if (officialWeekNum == 7)
        {
            CalculateOfficialRandomResults();
            CalculateOfficialToFinals();
        }
        else if(isWin && officialWeekNum >= 10)
        {
            FinalWinOnOfficial();
        }
        else if (!isWin)
        {
            ReleaseOfficial();
        }
    }

    //플레이오프 전 플레이서 승/패시
    public void CalculateOfficialPlayerResult(bool isWin, int setWin, int setLose)
    {
        //플레이어 경기결과 (위크넘버 갱신전)

        for (int i = 0; i < 4; i++)
        {
            int firstTeam = officialMatchInfo[officialWeekNum , (2 * i)];
            int secondTeam = officialMatchInfo[officialWeekNum, (2 * i) + 1];

            if (firstTeam == 7)
            {
                if (isWin)
                {
                    officialTeamDatas[firstTeam].win++;
                    officialTeamDatas[secondTeam].lose++;
                }
                else
                {
                    officialTeamDatas[firstTeam].lose++;
                    officialTeamDatas[secondTeam].win++;
                }

                officialTeamDatas[firstTeam].setWin += setWin;
                officialTeamDatas[firstTeam].setLose += setLose;
                officialTeamDatas[secondTeam].setWin += setLose;
                officialTeamDatas[secondTeam].setLose += setWin;

                officialMatchResult[officialWeekNum, (2 * i)] = setWin;
                officialMatchResult[officialWeekNum, (2 * i) + 1] = setLose;
                return;
            }
            else if (secondTeam == 7)
            {
                if (isWin)
                {
                    officialTeamDatas[firstTeam].lose++;
                    officialTeamDatas[secondTeam].win++;
                }
                else
                {
                    officialTeamDatas[firstTeam].win++;
                    officialTeamDatas[secondTeam].lose++;
                }


                officialTeamDatas[firstTeam].setWin += setLose;
                officialTeamDatas[firstTeam].setLose += setWin;
                officialTeamDatas[secondTeam].setWin += setWin;
                officialTeamDatas[secondTeam].setLose += setLose;

                officialMatchResult[officialWeekNum, (2 * i)] = setLose;
                officialMatchResult[officialWeekNum, (2 * i) + 1] = setWin;
                return;
            }
        }

    }

    public void CalculateOfficialRandomResults()
    {
        //플레이어 경기결과는 따로 넣어줘야함

        for (int i = 0; i < 4; i++)
        {
            int firstTeam = officialMatchInfo[officialWeekNum - 1, (2 * i)];
            int secondTeam = officialMatchInfo[officialWeekNum - 1, (2 * i) + 1];

            if (firstTeam != 7 && secondTeam != 7)
            {
                List<EnemyInfo> firstEnemies = enemyTeams[firstTeam];
                List<EnemyInfo> secondEnemies = enemyTeams[secondTeam];

                int firstValue = 0;
                int secondValue = 0;

                foreach (var item in firstEnemies)
                {
                    firstValue += item.hp / 10;
                    firstValue += item.atk;
                    firstValue -= (int)(item.atkRate * 10);
                    firstValue += (int)(item.range * 10);
                }

                foreach (var item in secondEnemies)
                {
                    secondValue += item.hp / 10;
                    secondValue += item.atk;
                    secondValue -= (int)(item.atkRate * 10);
                    secondValue += (int)(item.range * 10);
                }

                if (firstValue > secondValue)
                {
                    officialTeamDatas[firstTeam].win++;
                    officialTeamDatas[secondTeam].lose++;

                    officialTeamDatas[firstTeam].setWin += 2;
                    int randomLose = UnityEngine.Random.Range(0, 2);
                    officialTeamDatas[firstTeam].setLose += randomLose;
                    officialTeamDatas[secondTeam].setWin += randomLose;
                    officialTeamDatas[secondTeam].setLose += 2;

                    officialMatchResult[officialWeekNum - 1, (2 * i)] = 2;
                    officialMatchResult[officialWeekNum - 1, (2 * i) + 1] = randomLose;
                }
                else
                {
                    officialTeamDatas[firstTeam].lose++;
                    officialTeamDatas[secondTeam].win++;

                    officialTeamDatas[secondTeam].setWin += 2;
                    int randomLose = UnityEngine.Random.Range(0, 2);
                    officialTeamDatas[secondTeam].setLose += randomLose;
                    officialTeamDatas[firstTeam].setWin += randomLose;
                    officialTeamDatas[firstTeam].setLose += 2;

                    officialMatchResult[officialWeekNum - 1, (2 * i)] = randomLose;
                    officialMatchResult[officialWeekNum - 1, (2 * i) + 1] = 2;
                }
                

            }
        }
        
    }

    public void CalculateOfficialToFinals()
    {
        officialTeamDatas = OfficialTeamRankSort();
        int grade = 4;
        for (int i = 0; i < officialTeamDatas.Length; i++)
        {
            if (officialTeamDatas[i].isPlayer)
            {
                grade = i;
                break;
            }
        }

        switch (grade)
        {
            case 0:
                {
                    officialWeekNum = 9;
                    officialFinalMatchResultName[0, 1] = officialTeamDatas[3].name;
                    officialFinalMatchResultName[0, 0] = officialTeamDatas[2].name;
                    officialFinalMatchResultName[1, 0] = officialTeamDatas[1].name;
                    officialFinalMatchResultName[1, 1] = officialTeamDatas[2].name;
                    officialFinalMatchResultName[2, 0] = officialTeamDatas[0].name;
                    officialFinalMatchResultName[2, 1] = officialTeamDatas[1].name;

                    officialFinalMatchResult[0, 1] = UnityEngine.Random.Range(0, 2);
                    officialFinalMatchResult[0, 0] = 2;
                    officialFinalMatchResult[1, 1] = UnityEngine.Random.Range(0, 2);
                    officialFinalMatchResult[1, 0] = 2;
                }
                
                break;
            case 1:
                {
                    officialWeekNum = 8;
                    officialFinalMatchResultName[0, 1] = officialTeamDatas[3].name;
                    officialFinalMatchResultName[0, 0] = officialTeamDatas[2].name;
                    officialFinalMatchResultName[1, 0] = officialTeamDatas[1].name;
                    officialFinalMatchResultName[1, 1] = officialTeamDatas[2].name;
                    officialFinalMatchResultName[2, 0] = officialTeamDatas[0].name;

                    officialFinalMatchResult[0, 1] = UnityEngine.Random.Range(0,2);
                    officialFinalMatchResult[0, 0] = 2;
                }
                break;
            case 2:
            case 3:
                {
                    officialFinalMatchResultName[0, 1] = officialTeamDatas[3].name;
                    officialFinalMatchResultName[0, 0] = officialTeamDatas[2].name;
                    officialFinalMatchResultName[1, 0] = officialTeamDatas[1].name;
                    officialFinalMatchResultName[2, 0] = officialTeamDatas[0].name;
                }
                break;
            default:
                ReleaseOfficial();
                break;
        }
    }

    //플레이오프 승리
    public void CalculateOfficialFinalsPlayerResult(int setWin,int setLose)
    {
        switch (officialWeekNum)
        {
            case 8:
                {
                    officialFinalMatchResultName[2, 1] = teamName;

                    if (officialTeamDatas[2].isPlayer)
                    {
                        officialFinalMatchResult[1, 1] = setWin;
                        officialFinalMatchResult[1, 0] = setLose;
                        OfficialTeamData temp = officialTeamDatas[2];
                        officialTeamDatas[2] = officialTeamDatas[1];
                        officialTeamDatas[1] = temp;
                    }
                    else if (officialTeamDatas[1].isPlayer)
                    {
                        officialFinalMatchResult[1, 1] = setLose;
                        officialFinalMatchResult[1, 0] = setWin;
                    }
                }

                break;
            case 7:
                {
                    officialFinalMatchResultName[1, 1] = teamName;

                    if (officialTeamDatas[3].isPlayer)
                    {
                        officialFinalMatchResult[0, 1] = setWin;
                        officialFinalMatchResult[0, 0] = setLose;
                        OfficialTeamData temp = officialTeamDatas[3];
                        officialTeamDatas[3] = officialTeamDatas[2];
                        officialTeamDatas[2] = temp;
                    }
                    else if (officialTeamDatas[2].isPlayer)
                    {
                        officialFinalMatchResult[0, 1] = setLose;
                        officialFinalMatchResult[0, 0] = setWin;
                    }

                }
                break;
        }
    }

    //정규전 어떻게든 끝날때
    public void ReleaseOfficial()
    {
        isOnOfficial = false;
        endScrimmage = true;
        //officialLevel에 따라 보상
    }

    public void FinalWinOnOfficial()
    {
        officialWeekNum++;
        endScrimmage = true;
        isOnOfficial = false;
    }
}
public class PlayerConverter : JsonConverter<Player>
{
    public override Player ReadJson(JsonReader reader, Type objectType, Player existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jobj = JObject.Load(reader);
        Player player = new Player();
        player.ID = (float)jobj["ID"];
        player.name = (string)jobj["name"];
        player.code = (int)jobj["code"];
        player.type = (int)jobj["type"];
        player.grade = (int)jobj["grade"];
        player.level = (int)jobj["level"];
        player.maxLevel = (int)jobj["maxLevel"];
        player.breakthrough = (int)jobj["breakthrough"];
        player.skillLevel = (int)jobj["skillLevel"];
        player.gearCode = (int)jobj["gearCode"];
        player.gearLevel = (int)jobj["gearLevel"];
        player.xp = (float)jobj["xp"];
        player.maxXp = (float)jobj["maxXp"];
        player.condition = (int)jobj["condition"];
        player.cost = (int)jobj["cost"];
        player.potential = (int)jobj["potential"];
        int trainingCount = (int)jobj["trainingCount"];
        player.training = new List<int>();
        for (int i = 0; i < trainingCount; i++)
        {
            player.training.Add((int)jobj[$"training{i}"]);
        }

        return player;
    }

    public override void WriteJson(JsonWriter writer, Player value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("ID");
        writer.WriteValue(value.ID);
        writer.WritePropertyName("name");
        writer.WriteValue(value.name);
        writer.WritePropertyName("code");
        writer.WriteValue(value.code);
        writer.WritePropertyName("type");
        writer.WriteValue(value.type);
        writer.WritePropertyName("grade");
        writer.WriteValue(value.grade);
        writer.WritePropertyName("level");
        writer.WriteValue(value.level);
        writer.WritePropertyName("maxLevel");
        writer.WriteValue(value.maxLevel);
        writer.WritePropertyName("breakthrough");
        writer.WriteValue(value.breakthrough);
        writer.WritePropertyName("skillLevel");
        writer.WriteValue(value.skillLevel);
        writer.WritePropertyName("gearCode");
        writer.WriteValue(value.gearCode);
        writer.WritePropertyName("gearLevel");
        writer.WriteValue(value.gearLevel);
        writer.WritePropertyName("xp");
        writer.WriteValue(value.xp);
        writer.WritePropertyName("maxXp");
        writer.WriteValue(value.maxXp);
        writer.WritePropertyName("condition");
        writer.WriteValue(value.condition);
        writer.WritePropertyName("cost");
        writer.WriteValue(value.cost);
        writer.WritePropertyName("potential");
        writer.WriteValue(value.potential);
        writer.WritePropertyName("trainingCount");
        writer.WriteValue(value.training.Count);
        for (int i = 0; i < value.training.Count; i++)
        {
            writer.WritePropertyName($"training{i}");
            writer.WriteValue(value.training[i]);
        }

        writer.WriteEndObject();
    }
}

public class EnemyInfoConverter : JsonConverter<EnemyInfo>
{
    public override EnemyInfo ReadJson(JsonReader reader, Type objectType, EnemyInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jobj = JObject.Load(reader);
        EnemyInfo player = new EnemyInfo();

        player.name = (string)jobj["name"];
        player.code = (int)jobj["code"];
        player.type = (int)jobj["type"];
        player.grade = (int)jobj["grade"];
        player.info = (string)jobj["info"];
        player.weaponType = (int)jobj["weaponType"];
        player.atkType = (int)jobj["atkType"];
        player.kitingType = (int)jobj["kitingType"];
        player.mag = (int)jobj["mag"];
        player.reload = (float)jobj["reload"];
        player.hp = (int)jobj["hp"];
        player.atk = (int)jobj["atk"];
        player.atkRate = (float)jobj["atkRate"];
        player.moveSpeed = (float)jobj["moveSpeed"];
        player.sight = (float)jobj["sight"];
        player.range = (float)jobj["range"];
        player.critical = (float)jobj["critical"];
        player.accuracy = (float)jobj["accuracy"];
        player.reaction = (float)jobj["reaction"];
        player.detection = (float)jobj["detection"];

        return player;
    }

    public override void WriteJson(JsonWriter writer, EnemyInfo value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("name");
        writer.WriteValue(value.name);
        writer.WritePropertyName("code");
        writer.WriteValue(value.code);
        writer.WritePropertyName("type");
        writer.WriteValue(value.type);
        writer.WritePropertyName("grade");
        writer.WriteValue(value.grade);
        writer.WritePropertyName("uniqueSkill");
        writer.WriteValue(value.uniqueSkill);
        writer.WritePropertyName("info");
        writer.WriteValue(value.info);
        writer.WritePropertyName("weaponType");
        writer.WriteValue(value.weaponType);
        writer.WritePropertyName("atkType");
        writer.WriteValue(value.atkType);
        writer.WritePropertyName("kitingType");
        writer.WriteValue(value.kitingType);
        writer.WritePropertyName("mag");
        writer.WriteValue(value.mag);
        writer.WritePropertyName("reload");
        writer.WriteValue(value.reload);
        writer.WritePropertyName("hp");
        writer.WriteValue(value.hp);
        writer.WritePropertyName("atk");
        writer.WriteValue(value.atk);
        writer.WritePropertyName("atkRate");
        writer.WriteValue(value.atkRate);
        writer.WritePropertyName("moveSpeed");
        writer.WriteValue(value.moveSpeed);
        writer.WritePropertyName("sight");
        writer.WriteValue(value.sight);
        writer.WritePropertyName("range");
        writer.WriteValue(value.range);
        writer.WritePropertyName("critical");
        writer.WriteValue(value.critical);
        writer.WritePropertyName("accuracy");
        writer.WriteValue(value.accuracy);
        writer.WritePropertyName("reaction");
        writer.WriteValue(value.reaction);
        writer.WritePropertyName("detection");
        writer.WriteValue(value.detection);


        writer.WriteEndObject();
    }
}


public class OfficialTeamDataConverter : JsonConverter<OfficialTeamData>
{
    public override OfficialTeamData ReadJson(JsonReader reader, Type objectType, OfficialTeamData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jobj = JObject.Load(reader);
        OfficialTeamData player = new OfficialTeamData();

        player.name = (string)jobj["name"];
        player.index = (int)jobj["index"];
        player.win = (int)jobj["win"];
        player.lose = (int)jobj["lose"];
        player.setWin = (int)jobj["setWin"];
        player.setLose = (int)jobj["setLose"];
        player.isPlayer = (bool)jobj["isPlayer"];


        return player;
    }

    public override void WriteJson(JsonWriter writer, OfficialTeamData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("name");
        writer.WriteValue(value.name);
        writer.WritePropertyName("index");
        writer.WriteValue(value.index);
        writer.WritePropertyName("win");
        writer.WriteValue(value.win);
        writer.WritePropertyName("lose");
        writer.WriteValue(value.lose);
        writer.WritePropertyName("setWin");
        writer.WriteValue(value.setWin);
        writer.WritePropertyName("setLose");
        writer.WriteValue(value.setLose);
        writer.WritePropertyName("isPlayer");
        writer.WriteValue(value.isPlayer);


        writer.WriteEndObject();
    }
}


public class OfficialPlayerDataConverter : JsonConverter<OfficialPlayerData>
{
    public override OfficialPlayerData ReadJson(JsonReader reader, Type objectType, OfficialPlayerData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jobj = JObject.Load(reader);
        OfficialPlayerData player = new OfficialPlayerData();

        player.playCount = (int)jobj["playCount"];
        player.kill = (int)jobj["kill"];
        player.death = (int)jobj["death"];
        player.totalDamage = (int)jobj["totalDamage"];
        return player;
    }

    public override void WriteJson(JsonWriter writer, OfficialPlayerData value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("playCount");
        writer.WriteValue(value.playCount);
        writer.WritePropertyName("kill");
        writer.WriteValue(value.kill);
        writer.WritePropertyName("death");
        writer.WriteValue(value.death);
        writer.WritePropertyName("totalDamage");
        writer.WriteValue(value.totalDamage);

        writer.WriteEndObject();
    }
}

