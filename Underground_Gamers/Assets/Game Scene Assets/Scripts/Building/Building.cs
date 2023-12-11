using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Building : MonoBehaviour
{
    private List<AIController> aiControllers = new List<AIController>();

    public void AddAIController(AIController ai)
    {
        aiControllers.Add(ai);
    }
    public void PublishMissionTargetEvent()
    {
        foreach (var controller in aiControllers)
        {
            MissionTargetEventBus.Publish(controller.transform);
        }
    }
}
