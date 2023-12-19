using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float rotateSpeed = 30f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
