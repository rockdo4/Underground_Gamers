using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scrolling : MonoBehaviour
{
    public float duration = 2.0f;
    public float speed = 1.0f;
    public Color color = Color.white;

    private TextMeshPro textMesh;
    public Image image;
    private float timer = 0f;

    private Transform lookTarget;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Awake()
    {
        lookTarget = Camera.main.transform;
        textMesh = GetComponent<TextMeshPro>();
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (textMesh != null)
            textMesh.alpha = 1f - (timer / duration);

        if(image != null)
        {
            Color color = image.color;
            color.a = 1f - (timer / duration);
            image.color = color;
        }

        transform.position += Vector3.up * speed * Time.deltaTime;
        transform.LookAt(lookTarget);

        if (timer > duration)
        {
            ObjectPoolManager.Instance.textPool.ReturnObjectToPool(textMesh);
            //Destroy(gameObject);
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
