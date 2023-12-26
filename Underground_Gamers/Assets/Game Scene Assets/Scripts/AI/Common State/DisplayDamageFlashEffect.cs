using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDamageFlashEffect : MonoBehaviour, IAttackable
{
    private Color originColor = Color.white;
    private Color flashColor = Color.red;
    public float duration = 0.3f;
    private float startTime;
    private bool isHit;

    private void Update()
    {
        if(isHit)
        {
            float t = (Time.time - startTime) / duration;
            Color lerpColor = Color.Lerp(flashColor, originColor, t);
            FlashRenderer(lerpColor);
            if(t > 1)
                isHit = false;
        }
    }
    public void OnAttack(GameObject attacker, Attack attack)
    {
        isHit = true;
        startTime = Time.time;
        FlashRenderer(flashColor);
    }

    private void FlashRenderer(Color newColor)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            if (renderer.tag == "Effect")
                continue;
            renderer.material.color = newColor;
        }
    }
}
