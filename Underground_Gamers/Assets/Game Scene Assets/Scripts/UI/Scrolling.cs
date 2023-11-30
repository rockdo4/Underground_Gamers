using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scrolling : MonoBehaviour
{
    public float duration = 2.0f;
    public float speed = 1.0f;
    public Color color = Color.white;

    private TextMeshPro textMesh;
    private float timer = 0f;

    private Transform lookTarget;

    private void Awake()
    {
        lookTarget = Camera.main.transform;
        textMesh = GetComponent<TextMeshPro>();
    }


    private void Update()
    {
        timer += Time.deltaTime;
        textMesh.alpha = 1f - (timer / duration);

        transform.position += Vector3.up * speed * Time.deltaTime;
        transform.LookAt(lookTarget);

        if (timer > duration)
        {
            Destroy(gameObject);
        }
    }

    public void Set(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}
