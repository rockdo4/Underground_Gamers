using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{

    private void OnEnable()
    {
        Destroy(gameObject, 2f);
    }
}
