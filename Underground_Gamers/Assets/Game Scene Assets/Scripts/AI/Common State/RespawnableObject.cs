using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnableObject : MonoBehaviour, IRespawnable
{
    public Respawner respawner;
    public void ExecuteRespawn(GameObject dieObject)
    {
        CharacterStatus status = dieObject.GetComponent<CharacterStatus>();
        AIController aIController = dieObject.GetComponent<AIController>();
        CommandInfo commandInfo = aIController.aiCommandInfo;
        if (status == null)
            return;

        float currentRespawnTime = status.respawnTime + (float)status.deathCount * status.respawnTimeIncreaseRate + Time.time;

        if (commandInfo != null)
        {
            aIController.aiCommandInfo.OnRespawnUI(currentRespawnTime);
        }

        if (aIController.layer == LayerMask.GetMask("PC"))
            respawner.pcRespawnTimers.Add((aIController, currentRespawnTime));
        else if(aIController.layer == LayerMask.GetMask("NPC"))
            respawner.npcRespawnTimers.Add((aIController, currentRespawnTime));

        status.deathCount++;
    }
}
