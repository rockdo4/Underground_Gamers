using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Transform pcReSpawner;
    public Transform npcReSpawner;
    public AIManager aiManager;

    public List<(AIController, float)> pcRespawnTimers = new List<(AIController, float)>();
    public List<(AIController, float)> npcRespawnTimers = new List<(AIController, float)>();

    public float respawnRange = 1f;

    // Update is called once per frame
    void Update()
    {
        for (int i = pcRespawnTimers.Count - 1; i >= 0; --i)
        {
            if (pcRespawnTimers[i].Item2 <= Time.time)
            {
                pcRespawnTimers[i].Item1.transform.position = pcReSpawner.position +
                    new Vector3(Random.Range(-respawnRange, respawnRange), 0f, Random.Range(-respawnRange, respawnRange));

                pcRespawnTimers[i].Item1.aiCommandInfo.OffRespawnUI();
                pcRespawnTimers[i].Item1.SetState(States.Idle);
                pcRespawnTimers[i].Item1.gameObject.SetActive(true);
                pcRespawnTimers.RemoveAt(i);
            }
            else
            {
                float respawnTimeUI = pcRespawnTimers[i].Item2 - Time.time;
                pcRespawnTimers[i].Item1.aiCommandInfo.DisplayRespawnTime(respawnTimeUI);
            }
        }

        for (int i = npcRespawnTimers.Count - 1; i >= 0; --i)
        {
            if (npcRespawnTimers[i].Item2 <= Time.time)
            {
                npcRespawnTimers[i].Item1.transform.position = npcReSpawner.position +
                    new Vector3(Random.Range(-respawnRange, respawnRange), 0f, Random.Range(-respawnRange, respawnRange));

                npcRespawnTimers[i].Item1.SetState(States.Idle);
                npcRespawnTimers[i].Item1.gameObject.SetActive(true);
                npcRespawnTimers.RemoveAt(i);
            }
        }

        //foreach ((AIController controller, float respawnTime) in npcRespawnTimers)
        //{
        //    if (respawnTime <= Time.time)
        //    {
        //        controller.transform.position = npcReSpawner.position +
        //            new Vector3(Random.Range(-respawnRange, respawnRange), 0f, Random.Range(-respawnRange, respawnRange));
        //        controller.gameObject.SetActive(true);
        //        npcRespawnTimers.Remove((controller, respawnTime));
        //    }
        //}
    }
}
