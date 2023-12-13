using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayNode : MonoBehaviour
{
    public float distance = 8f;
    public float enterDistance = 13f;
    private void OnTriggerStay(Collider other)
    {
        AIController controller = other.GetComponent<AIController>();
        if (controller != null)
        {

            float dis = Vector3.Distance(controller.missionTarget.position, controller.transform.position);
            //if (other.name == "PC")
            //    Debug.Log(dis);
            if (dis < distance)
            {
                controller.RefreshBuilding();
                controller.SetMissionTarget(controller.missionTarget);
                if (controller.isAttack)
                    controller.SetState(States.MissionExecution);
                if (controller.isDefend)
                    controller.SetState(States.Retreat);    
            }
        }
    }
}
