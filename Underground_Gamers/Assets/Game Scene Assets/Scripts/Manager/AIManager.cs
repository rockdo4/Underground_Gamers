using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<AIController> pc;

    public List<AIController> npc;

    void Update()
    {
        if (!gameManager.IsStart)
            return;
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

    public void ResetAIState()
    {
        foreach (AIController controller in pc)
        {
            controller.SetState(States.Idle);
        }
        foreach (AIController controller in npc)
        {
            controller.SetState(States.Idle);
        }
    }
    public void RegisterMissionTargetEvent()
    {
        foreach (var controller in pc)
        {
            MissionTargetEventBus.Subscribe(controller.transform, controller.RefreshBuilding);
            controller.RefreshBuilding();
            //controller.SetState(States.Idle);
        }

        foreach (var controller in npc)
        {
            MissionTargetEventBus.Subscribe(controller.transform, controller.RefreshBuilding);
            controller.RefreshBuilding();
            //controller.SetState(States.Idle);
        }
    }
    public void SetAICanvas()
    {
        foreach (var controller in pc)
        {
            controller.canvas.SetClassIcon(controller.status.aiClass);
        }
        foreach (var controller in npc)
        {
            controller.canvas.SetClassIcon(controller.status.aiClass);
        }
    }
    public void SetActiveOffAI()
    {
        foreach (var ai in pc)
        {
            ai.gameObject.SetActive(false);
        }        
        foreach (var ai in npc)
        {
            ai.gameObject.SetActive(false);
        }
    }
    public void ResetAI()
    {
        SetActiveOffAI();
        pc.Clear();
        npc.Clear();
    }
}
