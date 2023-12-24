using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObject : MonoBehaviour, IDestroyable
{
    private GameManager gameManager;
    public ParticleSystem particlePrefab;
    public ParticleSystem[] aiParticlePrefabs;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void DestoryObject(GameObject attacker)
    {
        //if (gameManager != null)
        //    return;
        CharacterStatus status = GetComponent<CharacterStatus>();
        AIController controller = GetComponent<AIController>();
        if(controller != null)
        {
            controller.battleTarget = null;
            particlePrefab = aiParticlePrefabs[controller.aiIndex];

            if(gameManager.commandManager.currentAI == controller)
            {
                gameManager.commandManager.UnSelect();
            }
        }

        //Destroy(gameObject);
        if (status != null)
        {
            BattleTargetEventBus.Publish(status);
        }
        if(particlePrefab != null)
        {
            ParticleSystem effect = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            effect.Play();
            Destroy(effect, 2f);
        }
        gameObject.SetActive(false);
    }
}
