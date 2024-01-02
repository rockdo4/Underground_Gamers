using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationEffect : MonoBehaviour
{
    public float offset;
    public float scale = 1f;
    public float duration = 1f;

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        //SetOffsetNScale(offset, scale);
    }

    public void SetOffsetNScale(float offset, float scale)
    {
        Vector3 offsetPos = transform.position;
        offsetPos.y += offset;
        transform.position = offsetPos;

        transform.localScale *= scale;
    }
}
