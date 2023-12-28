using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHoleCastEffect : MonoBehaviour
{
    public TimeHoleExplosionEffect explosionEffectPrefab;

    // 캐스트
    [Header("캐스트 효과")]
    public float explosionOffset;
    public float explosionDuration = 1f;
    public float explosionScale;

    private SpeedBuff speedBuff;
    private AttackSpeedBuff attackSpeedBuff;
    private AIController ai;

    private void OnDisable()
    {
        TimeHoleExplosionEffect explosion = Instantiate(explosionEffectPrefab, transform.position, explosionEffectPrefab.transform.rotation);
        explosion.colOntime = explosionDuration - 0.1f;
        explosion.timer = Time.time;
        explosion.SetBuff(speedBuff, attackSpeedBuff, ai);
        Vector3 explosionPos = explosion.transform.position;
        explosionPos.y += explosionOffset;
        explosion.transform.position = explosionPos;
        explosion.transform.localScale *= explosionScale;
        Destroy(explosion.gameObject, explosionDuration);
    }

    public void SetBuff(SpeedBuff speedBuff, AttackSpeedBuff attackSpeedBuff, AIController ai)
    {
        this.speedBuff = speedBuff;
        this.attackSpeedBuff = attackSpeedBuff;
        this.ai = ai;
    }
}
