using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Transform pcReSpawner;
    public Transform npcReSpawner;
    public AIManager aiManager;

    public List<(AIController, float, float)> pcRespawnTimers = new List<(AIController, float, float)>();
    public List<(AIController, float)> npcRespawnTimers = new List<(AIController, float)>();

    public float respawnRange = 1f;
    public ParticleSystem[] aiParticlePrefabs;

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

                //aiManager.pc.Add(pcRespawnTimers[i].Item1);
                pcRespawnTimers[i].Item1.gameObject.SetActive(true);
                pcRespawnTimers[i].Item1.SetState(States.Idle);
                pcRespawnTimers[i].Item1.status.Respawn();
                pcRespawnTimers[i].Item1.SetInitialization();
                ParticleSystem effect = aiParticlePrefabs[pcRespawnTimers[i].Item1.aiIndex];
                if (effect != null)
                {
                    Instantiate(effect, pcRespawnTimers[i].Item1.transform.position, effect.transform.rotation);
                    effect.Play();
                }
                pcRespawnTimers.RemoveAt(i);
            }
            else
            {
                float respawnTimeUI = pcRespawnTimers[i].Item2 - Time.time;
                pcRespawnTimers[i].Item1.aiCommandInfo.DisplayRespawnTime(respawnTimeUI);
                pcRespawnTimers[i].Item1.aiCommandInfo.RefreshRespawnCoolTime(respawnTimeUI / (pcRespawnTimers[i].Item2 - pcRespawnTimers[i].Item3));
            }
        }

        for (int i = npcRespawnTimers.Count - 1; i >= 0; --i)
        {
            if (npcRespawnTimers[i].Item2 <= Time.time)
            {
                npcRespawnTimers[i].Item1.transform.position = npcReSpawner.position +
                    new Vector3(Random.Range(-respawnRange, respawnRange), 0f, Random.Range(-respawnRange, respawnRange));

                npcRespawnTimers[i].Item1.gameObject.SetActive(true);
                npcRespawnTimers[i].Item1.SetState(States.Idle);
                npcRespawnTimers[i].Item1.status.Respawn();
                npcRespawnTimers[i].Item1.SetInitialization();
                ParticleSystem effect = aiParticlePrefabs[npcRespawnTimers[i].Item1.aiIndex];
                if (effect != null)
                {
                    Instantiate(effect, npcRespawnTimers[i].Item1.transform.position, effect.transform.rotation);
                    effect.Play();
                }
                npcRespawnTimers.RemoveAt(i);
            }
        }
    }

    public void ClearRespawn()
    {
        pcRespawnTimers.Clear();
        npcRespawnTimers.Clear();
    }
}
