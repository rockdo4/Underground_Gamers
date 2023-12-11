using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<AIController> pc;

    public List<AIController> npc;

    public void RegisterMissionTargetEvent()
    {
        foreach (var controller in pc)
        {
            MissionTargetEventBus.Subscribe(controller.transform, controller.RefreshBuilding);
        }

        foreach (var controller in npc)
        {
            MissionTargetEventBus.Subscribe(controller.transform, controller.RefreshBuilding);
        }
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.Count > 0)
        {
            foreach (AIController controller in pc)
            {
                if (controller.status.IsLive)
                    controller.UpdateState();
            }
        }

        if (npc.Count > 0)
        {
            foreach (AIController controller in npc)
            {
                if (controller.status.IsLive)
                    controller.UpdateState();
            }
        }
    }
}
