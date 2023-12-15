
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTabs : MonoBehaviour
{
    public List<Toggle> stageTabs = new List<Toggle>();
    [SerializeField]
    private int LeagueType = 100;
    private Toggle firstStage;
    private StageTable st;
    private void Start()
    {
        st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
    }
    public void SetFirst()
    {
        if(st == null)
        {
            st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        }
        firstStage = stageTabs[0];
        int stageLevel = LeagueType;
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
            item.GetComponentInChildren<TMP_Text>(true).text = st.GetStageInfo(stageLevel).name;
        }
        firstStage.interactable = true;
        firstStage.isOn = true;
    }
}
