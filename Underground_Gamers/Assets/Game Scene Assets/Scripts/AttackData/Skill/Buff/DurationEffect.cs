using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationEffect : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public void SetOffsetNScale(float offset, float scale)
    {
        Vector3 offsetPos = transform.position;
        offsetPos.y += offset;
        transform.position = offsetPos;

        transform.localScale *= scale;
    }
}
