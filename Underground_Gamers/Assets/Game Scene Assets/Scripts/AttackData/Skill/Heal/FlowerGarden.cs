using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlowerGarden.Asset", menuName = "HealSkill/FlowerGarden")]
public class FlowerGarden : AttackDefinition
{
    [Header("Èú")]
    public GameObject castAreaEffectPrefab;
    public GameObject healEffectPrefab;
    public float healRateLevel1;
    public float healRateLevel2;
    public float healRateLevel3;
    public float delayTime;


}
