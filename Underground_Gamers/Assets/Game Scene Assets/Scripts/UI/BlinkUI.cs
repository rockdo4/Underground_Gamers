using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkUI : MonoBehaviour
{
    public float blinkSpeed;

    private Image image;
    public float alphaRate;

    private void Update()
    {
        Blink();
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Blink()
    {
        float alpha = Mathf.PingPong(blinkSpeed * Time.time, alphaRate);
        Color alphaColor = image.color;
        alphaColor.a = alpha;
        image.color= alphaColor;
    }
}
