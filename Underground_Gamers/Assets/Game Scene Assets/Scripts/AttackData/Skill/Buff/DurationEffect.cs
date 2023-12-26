using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationEffect : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
