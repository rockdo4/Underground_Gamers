using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditorInternal;
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
    public void Awake()
    {
        players = new List<GameObject>();
        enemys = new List<GameObject>();
    }
    public void RegistPlayers()
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

        List<Player> usePlayer = GamePlayerInfo.instance.usingPlayers;
        for (int i = 0; i < 5; i++)
        {
            var player = usePlayer[i];
            PlayerInfo playerInfo = PlayerLoadManager.instance.playerDatabase[PlayerLoadManager.instance.PlayerIndexSearch(player.code)];
            var madePlayer = Instantiate(playerObj);
            madePlayer.AddComponent<DontDestroy>();
            var madePlayerCharactor = Instantiate(Resources.Load<GameObject>(Path.Combine("SPUM", $"{player.code}")), madePlayer.transform);
            madePlayerCharactor.AddComponent<LookCameraRect>();

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
            madePlayer.SetActive(false);
            players.Add(madePlayer);
        }
    }

    public void MakePlayers()
    {
        var startPos = GameObject.FindGameObjectsWithTag("PlayerStartPos");
        var endPos = GameObject.FindGameObjectsWithTag("EnemyStartPos");
        var playerDestinations = GameObject.FindGameObjectsWithTag("PlayerDestination");

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
            var ai = player.GetComponent<AIController>();
            if (playerDestinations != null)
                ai.point = playerDestinations[Random.Range(0, playerDestinations.Length - 1)].transform;
            ai.SetDestination(ai.point.position);
        }
        if (players.Count > 0)
        {
            foreach (var player in players)
            {
                GameObject.FindGameObjectWithTag("AIManager").GetComponent<AIManager>().pc.Add(player.GetComponent<AIController>());
            }
        }

    }
}
