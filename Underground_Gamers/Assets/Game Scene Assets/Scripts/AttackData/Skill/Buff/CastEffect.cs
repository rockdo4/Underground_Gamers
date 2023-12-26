using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastEffect : MonoBehaviour
{
    private Transform parent;
    private GameObject durationEffectPrefab;
    private float offset;
    private float duration;



    private void OnDisable()
    {
        GameObject durationEffect = Instantiate(durationEffectPrefab, parent);
        Vector3 pos = durationEffect.transform.position;
        pos.y += offset;
        durationEffect.transform.position = pos;
        Destroy(durationEffect, duration);
        Destroy(gameObject);
    }
    public void SetDurationEffect(GameObject durationEffectPrefab, Transform parent, float duration, float offset)
    {
        this.durationEffectPrefab = durationEffectPrefab;
        this.parent = parent;
        this.duration = duration;
        this.offset = offset;
    }
}
