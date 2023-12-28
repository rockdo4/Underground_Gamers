using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public float timer;
    public float duration;
    public virtual void UpdateBuff(AIController ai)
    {
        if(timer + duration < Time.time)
        {
            RemoveBuff(ai);
            return;
        }
    }

    public abstract void ApplyBuff(AIController ai);
    public abstract void RemoveBuff(AIController ai);
}
