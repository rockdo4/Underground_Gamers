using Demo_Project;
using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum GameType
{
    Story,
    Official,
    Scrimmage,
}
public class GameInfo : MonoBehaviour
{
    public static GameInfo instance
    {
        get
        {
            if (gameInfo == null)
            {
                gameInfo = FindObjectOfType<GameInfo>();
            }
            return gameInfo;
        }
    }

    private static GameInfo gameInfo;

    public GameObject playerObj;
    public GameObject enemyObj;
    public int currentStage = 0;
    public float RandomSpawnRange = 1f;
    public GameType gameType = GameType.Story;
    public List<Player> entryPlayer { get; private set; } = new List<Player>();

    [HideInInspector]
    public int screammageLevel = 0;
    private List<GameObject> players;
    private List<GameObject> enemys;
    private PlayerTable pt;
    public List<int> entryMembersIndex = new List<int>();
    public List<int> benchMembersIndex = new List<int>();

    public string storyTeamName;

    //0부터 각각 경험치재화 1,2,3,4 ,골드, 크리스탈 순임
    private int[] rewards = new int[6] { 0, 0, 0, 0, 0, 0 };
    public int XpRewards { get; private set; }
    public void Awake()
    {
        players = new List<GameObject>();
        enemys = new List<GameObject>();
    }
    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
    }

    public void SetBenchMemberIndex(List<EntryPlayer> entryMembers)
    {
        benchMembersIndex.Clear();
        foreach (EntryPlayer entryPlayer in entryMembers)
        {
            benchMembersIndex.Add(entryPlayer.Index);

        }
        benchMembersIndex.Sort();
    }
    public void SetEntryMemeberIndex(List<EntryPlayer> entryMembers)
    {
        entryMembersIndex.Clear();

        foreach (EntryPlayer entryPlayer in entryMembers)
        {
            this.entryMembersIndex.Add(entryPlayer.Index);
        }

        entryMembersIndex.Sort();
    }

    public int SortByIndex(EntryPlayer a, EntryPlayer b)
    {
        return a.Index.CompareTo(b.Index);
    }

    public void SetEntryPlayer(List<int> entryIndex)
    {
        foreach (int index in entryIndex)
        {
            entryPlayer.Add(GamePlayerInfo.instance.GetOfficialPlayer(index));
        }
        GamePlayerInfo.instance.SaveFile();
    }

    public void ClearEntryPlayer()
    {
        entryPlayer.Clear();
    }

    public void ClearMembersIndex()
    {
        entryMembersIndex.Clear();
        benchMembersIndex.Clear();
    }

    public void StartGame()
    {
        GamePlayerInfo.instance.SaveFile();

        rewards = new int[6] { 0, 0, 0, 0, 0, 0 };

        switch (gameType)
        {
            case GameType.Story:
                {
                    foreach (Player player in GamePlayerInfo.instance.usingPlayers)
                    {
                        entryPlayer.Add(player);
                    }
                    var st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
                    StageInfo stageInfo = st.GetStageInfo(currentStage);
                    if (GamePlayerInfo.instance.cleardStage < currentStage)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            rewards[i] = stageInfo.rewards[i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            rewards[i] = stageInfo.rewards[i];
                        }
                    }
                    storyTeamName = stageInfo.teamName;
                    XpRewards = stageInfo.xp;
                }
                break;
            case GameType.Official:
                {

                }
                break;
            case GameType.Scrimmage:
                {
                    foreach (Player player in GamePlayerInfo.instance.usingPlayers)
                    {
                        entryPlayer.Add(player);
                    }
                    var st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
                    var rewardInfo = st.GetScrimmageRewards(screammageLevel);
                    for (int i = 0; i < 5; i++)
                    {
                        rewards[i] = rewardInfo[i];
                    }
                }
                break;
        }
    }

    public void SetOfficialPlayerCondition()
    {
        GamePlayerInfo.instance.GetOfficialPlayer(0).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(1).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(2).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(3).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(4).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(5).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(6).condition = RandomGetCondition();
        GamePlayerInfo.instance.GetOfficialPlayer(7).condition = RandomGetCondition();
    }

    public void MakePlayers()
    {
        GamePlayerInfo.instance.isOnSchedule = false;
        var stateDefines = DataTableManager.instance.stateDef;

        for (int i = 0; i < 5; i++)
        {
            var player = entryPlayer[i];
            // 테스트를 위한 코드
            if (pt == null)
                return;
            PlayerInfo playerInfo = pt.GetPlayerInfo(player.code);
            foreach (var item in player.training)
            {
                var ti = pt.GetTrainingInfo(item);
                ti.AddStats(playerInfo);
            }
            var madePlayer = Instantiate(playerObj);
            //madePlayer.AddComponent<DontDestroy>();
            
            var madePlayerCharactor = Instantiate(Resources.Load<GameObject>(Path.Combine("SPUM", $"{player.code}")), madePlayer.transform);
            madePlayerCharactor.AddComponent<LookCameraRect>();
            var outLine = madePlayerCharactor.AddComponent<Outlinable>();

            float charactorScale = madePlayer.transform.localScale.x;

            var ai = madePlayer.GetComponent<AIController>();
            ai.spum = madePlayerCharactor.GetComponent<SPUM_Prefabs>();
            var childs = madePlayerCharactor.GetComponentsInChildren<Transform>();
            foreach (var child in childs)
            {
                if (child.name == "ArmL")
                {
                    ai.leftHand = child;
                }
                else if (child.name == "ArmR")
                {
                    ai.rightHand = child;
                }

                //임시코드 나중에 바꿔야함!!
                ai.firePos = ai.rightHand;
            }
            AttackDefinition atkDef = stateDefines.attackDefs.Find(a => a.code == playerInfo.atkType).value;
            AttackDefinition skillDef = stateDefines.skillDatas.Find(a => a.code == playerInfo.UniqueSkillCode).value;
            ai.attackInfos[0] = atkDef;
            ai.attackInfos[1] = skillDef;
            ai.kitingInfo = stateDefines.kitingDatas.Find(a => a.code == playerInfo.kitingType).value;
            ai.code = playerInfo.code;
            ai.playerInfo = player;
            //ai.aiCommandInfo.SetPortraitInCommandInfo(player.code);
            ai.outlinable = outLine;

            var stat = madePlayer.GetComponent<CharacterStatus>();

            stat.AIName = playerInfo.name;
            stat.Hp = (int)pt.CalculateCurrStats(playerInfo.hp, player.level);
            stat.maxHp = stat.Hp;
            stat.speed = pt.CalculateCurrStats(playerInfo.moveSpeed, player.level);
            stat.sight = pt.CalculateCurrStats(playerInfo.sight, player.level);
            stat.range = pt.CalculateCurrStats(playerInfo.range, player.level);
            stat.reactionSpeed = pt.CalculateCurrStats(playerInfo.reactionSpeed, player.level) * 15;
            stat.damage = pt.CalculateCurrStats(playerInfo.atk, player.level);
            stat.cooldown = pt.CalculateCurrStats(playerInfo.atkRate, player.level);
            stat.critical = pt.CalculateCurrStats(playerInfo.critical, player.level);
            stat.chargeCount = playerInfo.magazine;
            stat.reloadCooldown = playerInfo.reloadingSpeed;
            stat.accuracyRate = pt.CalculateCurrStats(playerInfo.accuracy, player.level);
            stat.detectionRange = pt.CalculateCurrStats(playerInfo.detectionRange, player.level);
            stat.occupationType = (OccupationType)playerInfo.type;
            stat.distancePriorityType = DistancePriorityType.Closer;
            stat.illustration = pt.GetPlayerSprite(playerInfo.code);
            stat.aiClass = pt.playerTypeSprites[playerInfo.type - 1];
            stat.grade = pt.starsSprites[playerInfo.grade - 3];
            stat.lv = player.level;
            stat.maxLv = player.maxLevel;
            stat.xp = player.xp;
            stat.maxXp = player.maxXp;


            // 공격력, 공속, 이속
            if (GameInfo.instance.gameType == GameType.Official)
            {
                stat.condition = player.condition;
                stat.damage = UpdateStatsByCondition(stat.condition, stat.damage, true);
                stat.speed = UpdateStatsByCondition(stat.condition, stat.speed, true);
                stat.cooldown = UpdateStatsByCondition(stat.condition, stat.cooldown, false);
            }
            else
            {
                player.condition = 2;
                stat.condition = player.condition;
            }


            ai.SetInitialization();

            switch (stat.occupationType)
            {
                case OccupationType.Normal:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Assault:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Sniper:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Support:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                default:
                    break;
            }
            ai.priorityByDistance = stateDefines.distanceTargetPriorityDatas.Find(a => a.code == 0).value;

            madePlayer.SetActive(false);
            players.Add(madePlayer);
        }

        var startPos = GameObject.FindGameObjectsWithTag("PlayerStartPos");
        var endPos = GameObject.FindGameObjectsWithTag("EnemyStartPos");

        var buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        var aiManager = GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>();


        if (startPos.Length < 1)
        {
            return;
        }
        foreach (var player in players)
        {
            if (startPos == null) return;
            var spawnPos = startPos[Random.Range(0, startPos.Length - 1)].transform.position + new Vector3(Random.Range(-RandomSpawnRange, RandomSpawnRange), 0, Random.Range(-RandomSpawnRange, RandomSpawnRange));
            player.transform.position = spawnPos;
            player.SetActive(true);

            // 이곳에서 할당 안하도록 구성
            //var ai = player.GetComponent<AIController>();
            //if (buildingManager != null)
            //    ai.point = buildingManager[Random.Range(0, buildingManager.Length - 1)].transform;
            //ai.SetDestination(ai.point);

            var ai = player.GetComponent<AIController>();
            var portrait = player.GetComponent<Portrait>();
            //if (buildingManager != null)
            //    ai.point = buildingManager.GetAttackPoint(Line.Bottom, TeamType.PC);
            //ai.SetDestination(ai.point);
            ai.spum.gameObject.AddComponent<Outlinable>();
            ai.InitInGameScene();

            portrait.SetPortrait(ai.spum);
            player.GetComponent<LookCameraByScale>().SetPlayer();
            player.GetComponent<RespawnableObject>().respawner = GameObject.FindGameObjectWithTag("Respawner").GetComponent<Respawner>();

        }

        if (players.Count > 0)
        {
            foreach (var player in players)
            {
                aiManager.pc.Add(player.GetComponent<AIController>());
            }
        }

        players.Clear();
        switch (gameType)
        {
            case GameType.Story:
                MakeEnemys();
                break;
            case GameType.Official:
                MakeOfficialEnemys();
                break;
            case GameType.Scrimmage:
                MakeScrimmageEnemys();
                break;
        }
    }

    public void MakeEnemys()
    {
        var stateDefines = DataTableManager.instance.stateDef;
        var st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        var enemys = st.GetStageInfo(currentStage).enemys;
        for (int i = 0; i < 5; i++)
        {
            int player = enemys[i];
            EnemyInfo playerInfo = st.GetEnemyInfo(player);

            var madePlayer = Instantiate(enemyObj);
            var madePlayerCharactor = Instantiate(Resources.Load<GameObject>(Path.Combine("EnemySpum", $"{player}")), madePlayer.transform);
            madePlayerCharactor.AddComponent<LookCameraRect>();
            var outLine = madePlayerCharactor.AddComponent<Outlinable>();

            float charactorScale = madePlayer.transform.localScale.x;

            var ai = madePlayer.GetComponent<AIController>();
            ai.spum = madePlayerCharactor.GetComponent<SPUM_Prefabs>();
            var childs = madePlayerCharactor.GetComponentsInChildren<Transform>();
            foreach (var child in childs)
            {
                if (child.name == "ArmL")
                {
                    ai.leftHand = child;
                }
                else if (child.name == "ArmR")
                {
                    ai.rightHand = child;
                }

                //임시코드 나중에 바꿔야함!!
                ai.firePos = ai.rightHand;
            }
            AttackDefinition atkDef = stateDefines.attackDefs.Find(a => a.code == playerInfo.atkType).value;
            AttackDefinition skillDef = stateDefines.skillDatas.Find(a => a.code == playerInfo.uniqueSkill).value;
            ai.attackInfos[0] = atkDef;
            ai.attackInfos[1] = skillDef;
            ai.kitingInfo = stateDefines.kitingDatas.Find(a => a.code == playerInfo.kitingType).value;
            ai.code = playerInfo.code;
            //ai.aiCommandInfo.SetPortraitInCommandInfo(player.code);
            ai.outlinable = outLine;

            var stat = madePlayer.GetComponent<CharacterStatus>();

            stat.AIName = playerInfo.name;
            stat.Hp = playerInfo.hp;
            stat.maxHp = stat.Hp;

            stat.speed = playerInfo.moveSpeed;
            stat.sight = playerInfo.sight;
            stat.range = playerInfo.range;

            stat.reactionSpeed = playerInfo.reaction * 15;
            stat.damage = playerInfo.atk;
            stat.cooldown = playerInfo.atkRate;
            stat.critical = playerInfo.critical;
            stat.chargeCount = playerInfo.mag;
            stat.reloadCooldown = playerInfo.reload;
            stat.accuracyRate = playerInfo.accuracy;
            stat.detectionRange = playerInfo.detection;
            stat.aiClass = pt.playerTypeSprites[playerInfo.type - 1];
            stat.occupationType = (OccupationType)playerInfo.type;
            stat.distancePriorityType = DistancePriorityType.Closer;
            ai.SetInitialization();

            switch (stat.occupationType)
            {
                case OccupationType.Normal:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Assault:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Sniper:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Support:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                default:
                    break;
            }
            ai.priorityByDistance = stateDefines.distanceTargetPriorityDatas.Find(a => a.code == 0).value;

            madePlayer.SetActive(false);
            this.enemys.Add(madePlayer);
        }
        var startPos = GameObject.FindGameObjectsWithTag("EnemyStartPos");
        var endPos = GameObject.FindGameObjectsWithTag("PlayerStartPos");

        var buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        var aiManager = GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>();


        if (startPos.Length < 1)
        {
            return;
        }
        foreach (var player in this.enemys)
        {
            if (startPos == null) return;
            var spawnPos = startPos[Random.Range(0, startPos.Length - 1)].transform.position + new Vector3(Random.Range(-RandomSpawnRange, RandomSpawnRange), 0, Random.Range(-RandomSpawnRange, RandomSpawnRange));
            player.transform.position = spawnPos;
            player.SetActive(true);

            var ai = player.GetComponent<AIController>();
            var portrait = player.GetComponent<Portrait>();
            //if (buildingManager != null)
            //    ai.point = buildingManager.GetAttackPoint(Line.Bottom, TeamType.NPC);
            //ai.SetDestination(ai.point);
            ai.spum.gameObject.AddComponent<Outlinable>();
            ai.InitInGameScene();

            portrait.SetPortrait(ai.spum);
            player.GetComponent<LookCameraByScale>().SetPlayer();
            player.GetComponent<RespawnableObject>().respawner = GameObject.FindGameObjectWithTag("Respawner").GetComponent<Respawner>();

        }

        if (this.enemys.Count > 0)
        {
            foreach (var player in this.enemys)
            {
                aiManager.npc.Add(player.GetComponent<AIController>());
            }
        }

        //aiManager.RegisterMissionTargetEvent();
    }

    public void MakeOfficialEnemys()
    {
        var stateDefines = DataTableManager.instance.stateDef;
        var st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        List<EnemyInfo> enemies = new List<EnemyInfo>();
        if (GamePlayerInfo.instance.officialWeekNum < 7)
        {
            enemies = GamePlayerInfo.instance.enemyTeams
                    [GamePlayerInfo.instance.officialPlayerMatchInfo[GamePlayerInfo.instance.officialWeekNum]];
        }
        else
        {
            if (GamePlayerInfo.instance.officialTeamDatas[10 - GamePlayerInfo.instance.officialWeekNum].isPlayer)
            {
                enemies = GamePlayerInfo.instance.enemyTeams[GamePlayerInfo.instance.officialTeamDatas[9 - GamePlayerInfo.instance.officialWeekNum].index];
            }
            else
            {
                enemies = GamePlayerInfo.instance.enemyTeams[GamePlayerInfo.instance.officialTeamDatas[10 - GamePlayerInfo.instance.officialWeekNum].index];
            }
        }
        for (int i = 0; i < 5; i++)
        {
            EnemyInfo playerInfo = enemies[i];

            var madePlayer = Instantiate(enemyObj);
            var madePlayerCharactor = Instantiate(Resources.Load<GameObject>(Path.Combine("EnemySpum", $"{Random.Range(30001, 30030) + (Random.Range(1, 5) * 100)}")), madePlayer.transform);
            madePlayerCharactor.AddComponent<LookCameraRect>();
            var outLine = madePlayerCharactor.AddComponent<Outlinable>();

            float charactorScale = madePlayer.transform.localScale.x;

            var ai = madePlayer.GetComponent<AIController>();
            ai.spum = madePlayerCharactor.GetComponent<SPUM_Prefabs>();
            var childs = madePlayerCharactor.GetComponentsInChildren<Transform>();
            foreach (var child in childs)
            {
                if (child.name == "ArmL")
                {
                    ai.leftHand = child;
                }
                else if (child.name == "ArmR")
                {
                    ai.rightHand = child;
                }

                ai.firePos = ai.rightHand;
            }
            AttackDefinition atkDef = stateDefines.attackDefs.Find(a => a.code == playerInfo.atkType).value;
            AttackDefinition skillDef = stateDefines.skillDatas.Find(a => a.code == playerInfo.uniqueSkill).value;
            ai.attackInfos[0] = atkDef;
            ai.attackInfos[1] = skillDef;
            ai.kitingInfo = stateDefines.kitingDatas.Find(a => a.code == playerInfo.kitingType).value;
            ai.code = playerInfo.code;
            ai.outlinable = outLine;

            var stat = madePlayer.GetComponent<CharacterStatus>();

            stat.AIName = playerInfo.name;
            stat.Hp = playerInfo.hp;
            stat.maxHp = stat.Hp;

            stat.speed = playerInfo.moveSpeed;
            stat.sight = playerInfo.sight;
            stat.range = playerInfo.range;

            stat.reactionSpeed = playerInfo.reaction * 15;
            stat.damage = playerInfo.atk;
            stat.cooldown = playerInfo.atkRate;
            stat.critical = playerInfo.critical;
            stat.chargeCount = playerInfo.mag;
            stat.reloadCooldown = playerInfo.reload;
            stat.accuracyRate = playerInfo.accuracy;
            stat.detectionRange = playerInfo.detection;
            stat.aiClass = pt.playerTypeSprites[playerInfo.type - 1];
            stat.occupationType = (OccupationType)playerInfo.type;
            stat.distancePriorityType = DistancePriorityType.Closer;
            ai.SetInitialization();

            switch (stat.occupationType)
            {
                case OccupationType.Normal:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Assault:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Sniper:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Support:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                default:
                    break;
            }
            ai.priorityByDistance = stateDefines.distanceTargetPriorityDatas.Find(a => a.code == 0).value;

            madePlayer.SetActive(false);
            this.enemys.Add(madePlayer);
        }
        var startPos = GameObject.FindGameObjectsWithTag("EnemyStartPos");
        var endPos = GameObject.FindGameObjectsWithTag("PlayerStartPos");

        var buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        var aiManager = GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>();


        if (startPos.Length < 1)
        {
            return;
        }
        foreach (var player in this.enemys)
        {
            if (startPos == null) return;
            var spawnPos = startPos[Random.Range(0, startPos.Length - 1)].transform.position + new Vector3(Random.Range(-RandomSpawnRange, RandomSpawnRange), 0, Random.Range(-RandomSpawnRange, RandomSpawnRange));
            player.transform.position = spawnPos;
            player.SetActive(true);

            var ai = player.GetComponent<AIController>();
            var portrait = player.GetComponent<Portrait>();
            if (buildingManager != null)
                ai.point = buildingManager.GetAttackPoint(Line.Bottom, TeamType.NPC);
            ai.SetDestination(ai.point);
            ai.spum.gameObject.AddComponent<Outlinable>();
            ai.InitInGameScene();

            portrait.SetPortrait(ai.spum);
            player.GetComponent<LookCameraByScale>().SetPlayer();
            player.GetComponent<RespawnableObject>().respawner = GameObject.FindGameObjectWithTag("Respawner").GetComponent<Respawner>();

        }

        if (this.enemys.Count > 0)
        {
            foreach (var player in this.enemys)
            {
                aiManager.npc.Add(player.GetComponent<AIController>());
            }
        }

        this.enemys.Clear();
    }
    public void MakeScrimmageEnemys()
    {
        var stateDefines = DataTableManager.instance.stateDef;
        var st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        var str = DataTableManager.instance.Get<StringTable>(DataType.String);
        var enemys = st.GetScrimmageEnemies(screammageLevel);
        var randIds = st.GenerateRandomNumbers(0, 99, 5);
        for (int i = 0; i < 5; i++)
        {
            EnemyInfo playerInfo = enemys[i];
            playerInfo.name = $"{str.Get($"random_player_name{randIds[i]}")}";
            var madePlayer = Instantiate(enemyObj);
            var madePlayerCharactor = Instantiate(Resources.Load<GameObject>(Path.Combine("EnemySpum", $"{Random.Range(30001, 30030) + (Random.Range(1, 5) * 100)}")), madePlayer.transform);
            madePlayerCharactor.AddComponent<LookCameraRect>();
            var outLine = madePlayerCharactor.AddComponent<Outlinable>();

            float charactorScale = madePlayer.transform.localScale.x;

            var ai = madePlayer.GetComponent<AIController>();
            ai.spum = madePlayerCharactor.GetComponent<SPUM_Prefabs>();
            var childs = madePlayerCharactor.GetComponentsInChildren<Transform>();
            foreach (var child in childs)
            {
                if (child.name == "ArmL")
                {
                    ai.leftHand = child;
                }
                else if (child.name == "ArmR")
                {
                    ai.rightHand = child;
                }

                //임시코드 나중에 바꿔야함!!
                ai.firePos = ai.rightHand;
            }
            AttackDefinition atkDef = stateDefines.attackDefs.Find(a => a.code == playerInfo.atkType).value;
            AttackDefinition skillDef = stateDefines.skillDatas.Find(a => a.code == playerInfo.uniqueSkill).value;
            ai.attackInfos[0] = atkDef;
            ai.attackInfos[1] = skillDef;
            ai.kitingInfo = stateDefines.kitingDatas.Find(a => a.code == playerInfo.kitingType).value;
            ai.code = playerInfo.code;
            //ai.aiCommandInfo.SetPortraitInCommandInfo(player.code);
            ai.outlinable = outLine;

            var stat = madePlayer.GetComponent<CharacterStatus>();

            stat.AIName = playerInfo.name;
            stat.Hp = playerInfo.hp;
            stat.maxHp = stat.Hp;

            stat.speed = playerInfo.moveSpeed;
            stat.sight = playerInfo.sight;
            stat.range = playerInfo.range;

            stat.reactionSpeed = playerInfo.reaction * 15;
            stat.damage = playerInfo.atk;
            stat.cooldown = playerInfo.atkRate;
            stat.critical = playerInfo.critical;
            stat.chargeCount = playerInfo.mag;
            stat.reloadCooldown = playerInfo.reload;
            stat.accuracyRate = playerInfo.accuracy;
            stat.detectionRange = playerInfo.detection;
            stat.aiClass = pt.playerTypeSprites[playerInfo.type - 1];
            stat.occupationType = (OccupationType)playerInfo.type;
            stat.distancePriorityType = DistancePriorityType.Closer;
            ai.SetInitialization();

            switch (stat.occupationType)
            {
                case OccupationType.Normal:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Assault:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Sniper:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                case OccupationType.Support:
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 1).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 2).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 3).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 4).value);
                    ai.priorityByOccupation.Add(stateDefines.occupationTargetPriorityDatas.Find(a => a.code == 0).value);
                    break;
                default:
                    break;
            }
            ai.priorityByDistance = stateDefines.distanceTargetPriorityDatas.Find(a => a.code == 0).value;

            madePlayer.SetActive(false);
            this.enemys.Add(madePlayer);
        }
        var startPos = GameObject.FindGameObjectsWithTag("EnemyStartPos");
        var endPos = GameObject.FindGameObjectsWithTag("PlayerStartPos");

        var buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();
        var aiManager = GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>();


        if (startPos.Length < 1)
        {
            return;
        }
        foreach (var player in this.enemys)
        {
            if (startPos == null) return;
            var spawnPos = startPos[Random.Range(0, startPos.Length - 1)].transform.position + new Vector3(Random.Range(-RandomSpawnRange, RandomSpawnRange), 0, Random.Range(-RandomSpawnRange, RandomSpawnRange));
            player.transform.position = spawnPos;
            player.SetActive(true);

            var ai = player.GetComponent<AIController>();
            var portrait = player.GetComponent<Portrait>();
            if (buildingManager != null)
                ai.point = buildingManager.GetAttackPoint(Line.Bottom, TeamType.NPC);
            ai.SetDestination(ai.point);
            ai.spum.gameObject.AddComponent<Outlinable>();
            ai.InitInGameScene();

            portrait.SetPortrait(ai.spum);
            player.GetComponent<LookCameraByScale>().SetPlayer();
            player.GetComponent<RespawnableObject>().respawner = GameObject.FindGameObjectWithTag("Respawner").GetComponent<Respawner>();

        }

        if (this.enemys.Count > 0)
        {
            foreach (var player in this.enemys)
            {
                aiManager.npc.Add(player.GetComponent<AIController>());
            }
        }

        this.enemys.Clear();
    }

    public void DeletePlayers()
    {
        foreach (var player in players)
        {
            Destroy(player);
        }
        foreach (var player in enemys)
        {
            Destroy(player);
        }
        players.Clear();
        enemys.Clear();
    }

    public void WinReward()
    {
        switch (gameType)
        {
            case GameType.Story:
                if (GamePlayerInfo.instance.cleardStage < currentStage)
                {
                    GamePlayerInfo.instance.cleardStage = currentStage;
                    switch (currentStage)
                    {
                        case 101:
                            GamePlayerInfo.instance.AddTutorial(1);
                            break;
                        case 102:
                            GamePlayerInfo.instance.AddTutorial(2);
                            break;
                        case 103:
                            GamePlayerInfo.instance.AddTutorial(3);
                            break;
                        case 104:
                            GamePlayerInfo.instance.AddTutorial(4);
                            break;
                        case 105:
                            GamePlayerInfo.instance.AddTutorial(5);
                            break;
                        default:
                            break;
                    }
                }
                GamePlayerInfo.instance.AddMoney(rewards[4], rewards[5], 0);
                GamePlayerInfo.instance.GetXpItems(rewards[0], rewards[1], rewards[2], rewards[3]);
                GamePlayerInfo.instance.LevelUpTeams(XpRewards);
                break;
            case GameType.Official:
                break;
            case GameType.Scrimmage:
                GamePlayerInfo.instance.AddMoney(rewards[4], rewards[5], 0);
                GamePlayerInfo.instance.GetXpItems(rewards[0], rewards[1], rewards[2], rewards[3]);
                break;
        }
    }

    public void UpdateOfficialPlayerResults(int index, int kill, int death, int totalDamage)
    {
        GamePlayerInfo.instance.officialPlayerDatas[index].playCount++;
        GamePlayerInfo.instance.officialPlayerDatas[index].kill += kill;
        GamePlayerInfo.instance.officialPlayerDatas[index].death += death;
        GamePlayerInfo.instance.officialPlayerDatas[index].totalDamage += totalDamage;
    }

    public int RandomGetCondition()
    {
        float randCondition = Random.Range(0f, 10f);

        if (randCondition >= 0 && randCondition < 1f)
        {
            return 4;
        }
        else if (randCondition >= 1f && randCondition < 3f)
        {
            return 3;
        }
        else if (randCondition >= 3f && randCondition < 7f)
        {
            return 2;
        }
        else if (randCondition >= 7f && randCondition < 9f)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public float UpdateStatsByCondition(int condition, float value, bool isLargerBetter)
    {
        if (isLargerBetter)
        {
            value += condition switch
            {
                0 => value * 0.05f,
                1 => value * 0.03f,
                2 => value * 0f,
                3 => value * -0.08f,
                4 => value * -0.1f,
                _ => value * 0f
            };
        }
        else
        {
            value -= condition switch
            {
                0 => value * 0.05f,
                1 => value * 0.03f,
                2 => value * 0f,
                3 => value * -0.08f,
                4 => value * -0.1f,
                _ => value * 0f
            };
        }
        return value;
    }
}
