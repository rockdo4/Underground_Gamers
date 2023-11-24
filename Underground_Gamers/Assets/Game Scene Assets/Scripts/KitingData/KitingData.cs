using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KitingData.Asset", menuName = "KitingData/KitingData")]
public class KitingData : ScriptableObject
{
    public float kitingSpeed;
    public float kitingCoolTime;

    public virtual void UpdateKiting(Transform target, AIController ctrl)
    {
    }
}
