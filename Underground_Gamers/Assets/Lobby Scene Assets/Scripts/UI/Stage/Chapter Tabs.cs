using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTabs : MonoBehaviour
{
    public List<Toggle> stageTabs = new List<Toggle>();
    [SerializeField]
    private int LeagueType = 100;
    private Toggle firstStage;

    public void SetFirst()
    {
        firstStage = stageTabs[0];
        int stageLevel = LeagueType - 1;
        int currstage = GamePlayerInfo.instance.cleardStage;
        foreach (var item in stageTabs)
        {
            if (currstage >= stageLevel++)
            {
                item.interactable = true;
                firstStage = item;
            }
            else
            {
                item.interactable = false;
            }
        }
        firstStage.interactable = true;
        firstStage.isOn = true;
    }
}
