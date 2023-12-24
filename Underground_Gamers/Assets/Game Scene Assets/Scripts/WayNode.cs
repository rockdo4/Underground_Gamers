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
                if (!controller.isMission && controller.isAttack)
                {
                    Transform attackTarget = controller.buildingManager.GetAttackPoint(controller.currentLine, controller.teamIdentity.teamType);
                    controller.SetMissionTarget(attackTarget);
                    controller.isMission = true;

                    // if문 분기
                    controller.SetState(States.MissionExecution);
                }

                if (!controller.isRetreat && controller.isDefend)
                {
                    controller.isRetreat = true;
                    Transform defendTarget = controller.buildingManager.GetDefendPoint(controller.currentLine, controller.teamIdentity.teamType).GetComponent<Building>().defendPoint;
                    controller.SetMissionTarget(defendTarget);

                    // if문
                    controller.SetState(States.Retreat);
                }
            }
        }
    }
}
