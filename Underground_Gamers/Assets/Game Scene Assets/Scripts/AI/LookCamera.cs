using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public void LookAtCamera()
    {
        transform.LookAt(Camera.main.transform);
    }
}
