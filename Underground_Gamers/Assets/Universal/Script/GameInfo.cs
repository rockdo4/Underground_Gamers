using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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
    public static int currentStage = 0;
    public float RandomSpawnRange = 1f;

    private List<GameObject> players;
    private List<GameObject> enemys;
    private PlayerTable pt;
    public void Awake()
    {
        players = new List<GameObject>();
        enemys = new List<GameObject>();
    }
    private void Start()
    {
        pt = DataTableManager.instance.Get<PlayerTable>(DataType.Player);
    }
    public void RegistPlayers()
    {
        var stateDefines = DataTableManager.instance.stateDef;
        List<Player> usePlayer = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 5; i++)
        {
            var player = usePlayer[i];
            PlayerInfo playerInfo = pt.GetPlayerInfo(player.code);
            var madePlayer = Instantiate(playerObj);
            madePlayer.AddComponent<DontDestroy>();
            var madePlayerCharactor = Instantiate(Resources.Load<GameObject>(Path.Combine("SPUM", $"{player.code}")), madePlayer.transform);
            madePlayerCharactor.AddComponent<LookCameraRect>();
            float charactorScale = madePlayerCharactor.transform.localScale.x;

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
            ai.SetInitialization();

            var stat = madePlayer.GetComponent<CharacterStatus>();

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
            stat.detectionRange = pt.CalculateCurrStats(playerInfo.detectionRange, player.level) * charactorScale;
            stat.occupationType = (OccupationType)playerInfo.type;
            stat.distancePriorityType = DistancePriorityType.Closer;

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
    }

    public void MakePlayers()
    {
        var startPos = GameObject.FindGameObjectsWithTag("PlayerStartPos");
        var endPos = GameObject.FindGameObjectsWithTag("EnemyStartPos");

        // 수정 할 곳
        var buildingManager = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<BuildingManager>();

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
            if (buildingManager != null)
                ai.point = buildingManager.GetPoint(Line.Bottom, TeamType.PC);
            ai.SetDestination(ai.point);

            portrait.SetPortrait(ai.spum);
            player.GetComponent<LookCameraByScale>().SetPlayer();
            player.GetComponent<RespawnableObject>().respawner = GameObject.FindGameObjectWithTag("Respawner").GetComponent<Respawner>();
        }

        if (players.Count > 0)
        {
            foreach (var player in players)
            {
                GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>().pc.Add(player.GetComponent<AIController>());
            }
        }
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
}
