using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObject : MonoBehaviour, IDestroyable
{
    public ParticleSystem particlePrefab;
    public ParticleSystem[] aiParticlePrefabs;

    public void DestoryObject()
    {
        CharacterStatus status = GetComponent<CharacterStatus>();
        AIController controller = GetComponent<AIController>();
        if(controller != null)
        {
            particlePrefab = aiParticlePrefabs[controller.colorIndex];
        }

        //Destroy(gameObject);
        if (status != null)
        {
            TargetEventBus.Publish(status);
        }
        if(particlePrefab != null)
        {
            ParticleSystem effect = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            effect.Play();
            //Destroy(effect, 2f);
        }
        gameObject.SetActive(false);
    }
}
