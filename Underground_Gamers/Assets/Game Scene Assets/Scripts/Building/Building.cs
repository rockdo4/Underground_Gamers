
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private List<AIController> aiControllers = new List<AIController>();
    public GameObject hpBar;
    public Transform defendPoint;

    public void AddAIController(AIController ai)
    {
        aiControllers.Add(ai);
    }

    public void ReleaseAIController(AIController ai)
    {
        aiControllers.Remove(ai);
    }

    public void PublishMissionTargetEvent()
    {
        foreach (var controller in aiControllers)
        {
            MissionTargetEventBus.Publish(controller.transform);
        }
    }
}
