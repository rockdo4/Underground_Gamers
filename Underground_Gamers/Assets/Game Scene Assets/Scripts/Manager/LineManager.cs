using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public List<AIController> pcTopLiner = new List<AIController>();
    public List<AIController> pcBottomLiner = new List<AIController>();
    public List<AIController> npcTopLiner = new List<AIController>();
    public List<AIController> npcBottomLiner = new List<AIController>();

    public void JoiningLine(AIController ai)
    {
        List<AIController> liner = null;
        switch (ai.teamIdentity.teamType)
        {
            case TeamType.PC:
                liner = ai.currentLine switch
                {
                    Line.Top => pcTopLiner,
                    Line.Bottom => pcBottomLiner,
                    _ => pcTopLiner
                };

                break;
            case TeamType.NPC:
                liner = ai.currentLine switch
                {
                    Line.Top => npcTopLiner,
                    Line.Bottom => npcBottomLiner,
                    _ => npcTopLiner
                };
                break;
        }
        ai.currentLinerInfo.Remove(ai);
        liner.Add(ai);
        ai.currentLinerInfo = liner;
    }
}
