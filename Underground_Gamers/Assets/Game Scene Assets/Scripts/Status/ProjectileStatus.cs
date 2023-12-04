using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStatus : CharacterStatus
{
    [Header("투사체")]
    public float lifeCycle;
    [Header("직선 투사체")]
    public bool isPenetrating;
    [Header("범위 투사체")]
    public bool isAreaAttack;
    public float explosionRange;

}
