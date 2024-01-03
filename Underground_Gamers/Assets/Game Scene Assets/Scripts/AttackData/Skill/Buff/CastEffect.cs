using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastEffect : MonoBehaviour
{
    private Transform parent;
    private DurationEffect durationEffectPrefab;
    private float offset;
    private float duration;
    private float scale;

    private void OnDisable()
    {
        DurationEffect durationEffect = Instantiate(durationEffectPrefab, parent);
        Vector3 pos = durationEffect.transform.position;
        pos.y += offset;
        durationEffect.transform.position = pos;
        durationEffect.transform.localScale *= scale;
        Destroy(durationEffect.gameObject, duration);
        Destroy(gameObject);
    }
    public void SetDurationEffect(DurationEffect durationEffectPrefab, Transform parent, float duration, float offset, float scale)
    {
        this.durationEffectPrefab = durationEffectPrefab;
        this.parent = parent;
        this.duration = duration;
        this.offset = offset;
        this.scale = scale;
    }
}
