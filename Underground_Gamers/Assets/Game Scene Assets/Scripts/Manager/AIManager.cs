using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<AIController> pc;

    public List<AIController> npc;

    // Update is called once per frame
    void Update()
    {
        if(pc.Count >0)
        {
            foreach(AIController controller in pc)
            {
                controller.UpdateState();
            }
        }        
        
        if(npc.Count >0)
        {
            foreach(AIController controller in npc)
            {
                controller.UpdateState();
            }
        }
    }
}
