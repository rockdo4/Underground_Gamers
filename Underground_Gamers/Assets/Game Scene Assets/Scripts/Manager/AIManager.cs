using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public AIController pc1;

    public AIController npc1;

    // Update is called once per frame
    void Update()
    {
        pc1.UpdateState();
        npc1.UpdateState();
    }
}
