using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameManager gameManager;

    public void SelectLineByInit(AIController ai)
    {
        int selectLine = Random.Range(0, 2);

        ai.currentLine = selectLine switch
        {
            0 => Line.Bottom,
            1 => Line.Top,
            _ => Line.Bottom,
        };

        if (gameManager.lineManager.npcTopLiner.Count == 4)
        {
            ai.currentLine = Line.Bottom;
        }
        if (gameManager.lineManager.npcBottomLiner.Count == 4)
        {
            ai.currentLine = Line.Top;
        }
        gameManager.lineManager.JoiningLine(ai);
    }
    public void SelectLineByRate(AIController ai)
    {
        int topCount = gameManager.lineManager.npcTopLiner.Count;
        int bottomCount = gameManager.lineManager.npcBottomLiner.Count;
        int allCount = topCount + bottomCount;
        float lineRate = (float)(allCount - topCount) / (float)allCount;

        // 1 ~ 0
        bool isTopLine = Random.value < lineRate;
        if(isTopLine)
        {
            ai.currentLine = Line.Top;
        }
        if (gameManager.lineManager.npcTopLiner.Count == 4)
        {
            ai.currentLine = Line.Bottom;
        }
        if (gameManager.lineManager.npcBottomLiner.Count == 4)
        {
            ai.currentLine = Line.Top;
        }
    }    
    
    public void AllSelectLineByRate()
    {
        List<AIController> npcs = gameManager.aiManager.npc;
        foreach(AIController ai in npcs)
        {
            int topCount = gameManager.lineManager.npcTopLiner.Count;
            int bottomCount = gameManager.lineManager.npcBottomLiner.Count;
            int allCount = topCount + bottomCount;
            float lineRate = (allCount - topCount) / allCount;

            // 1 ~ 0
            bool isTopLine = Random.value < lineRate;

            if (gameManager.lineManager.npcTopLiner.Count == 4)
            {
                ai.currentLine = Line.Bottom;
            }
            if (gameManager.lineManager.npcBottomLiner.Count == 4)
            {
                ai.currentLine = Line.Top;
            }
        }
    }
}
