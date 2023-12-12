using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTabs : MonoBehaviour
{
    private ScheduleUIStory stageSelectManager;
    [SerializeField]
    private int ChapterCode = 0;

    private List<GameObject> stageLists = new List<GameObject>();
    private ToggleGroup toggleGroup;

    private void Awake()
    {
        toggleGroup = gameObject.AddComponent<ToggleGroup>();
        toggleGroup.allowSwitchOff = false;
    }
    public void OnEnter()
    {
        foreach (GameObject go in stageLists) 
        {
            go.SetActive(true);
        }
    }
    public void OnExit()
    {
        foreach (GameObject go in stageLists)
        {
            go.SetActive(false);
        }
    }

    public void MakeButtons(GameObject buttonPrefab,Transform transform,ScheduleUIStory stageSelectManager)
    {
        for (int i = 0; i < 5; i++) 
        { 
            var toggleObj = Instantiate(buttonPrefab, transform);
            var toggle = toggleObj.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            toggle.isOn = false;
            this.stageSelectManager = stageSelectManager;
            toggle.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    this.stageSelectManager.SetStage(0);
                }
            }
            );
            toggleObj.SetActive(false);
            stageLists.Add(toggleObj);
        }
    }
}
