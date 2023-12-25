using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingLevelUp : MonoBehaviour
{
    public float duration = 2.0f;
    public float speed = 1.0f;
    public Color color = Color.white;

    public Image image;
    private float timer = 0f;


    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        Color color = image.color;
        color.a = 1f - (timer / duration);
        image.color = color;

        transform.position += Vector3.up * speed * Time.unscaledDeltaTime;


        if (timer > duration)
        {
            Clear();
        }
    }

    public void Set(Color color)
    {
        image.color = color;
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}
