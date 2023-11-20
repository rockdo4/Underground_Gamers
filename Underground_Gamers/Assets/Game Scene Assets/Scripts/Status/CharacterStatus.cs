using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public int maxHp;
    public float speed;
    public float sight;                 // 시야
    public float evasionRate;       // 회피율
    public float reactionSpeed;     // 반응속도

    public int damage;

    public int armor;

    public bool IsLive { get; set; } = true;
    public int Hp { get; set; }

    private void Awake()
    {
        Hp = maxHp;
    }
}
