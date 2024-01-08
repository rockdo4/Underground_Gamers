
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
    private StringTable str;
    private void Start()
    {
        st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
        str = DataTableManager.instance.Get<StringTable>(DataType.String);
    }
    public void SetFirst()
    {
        if(st == null)
        {
            st = DataTableManager.instance.Get<StageTable>(DataType.Stage);
            str = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        firstStage = stageTabs[0];
        int stageLevel = LeagueType;
        int currstage = GamePlayerInfo.instance.cleardStage;
        int count = 1;
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
            var nameText = item.GetComponentInChildren<TMP_Text>(true);
           nameText.text = str.Get($"story_name_{stageLevel}");
            if (item.interactable)
            {
                nameText.color = Color.white;
            }
            else
            {
                nameText.color = Color.gray;
            }
            item.GetComponent<StageTabs>().stageNum.text = $"{LeagueType / 100} - {count++}";

        }
        firstStage.interactable = true;
        firstStage.isOn = true;
        firstStage.onValueChanged.Invoke(firstStage);
        firstStage.GetComponentInChildren<TMP_Text>(true).color = Color.white;
    }
}
