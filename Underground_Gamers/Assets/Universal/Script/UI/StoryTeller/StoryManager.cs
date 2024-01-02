using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditorInternal;
using static UnityEditor.Progress;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance
    {
        get
        {
            if (storyManager == null)
            {
                storyManager = FindObjectOfType<StoryManager>();
            }
            return storyManager;
        }
    }

    private static StoryManager storyManager;

    public float textSpeed;
    [Space(10f)]
    [SerializeField]
    private List<StorySetter> storyDataDefines;
    [SerializeField]
    private List<int> tutorialCodeDefines;
    private StorySetter currentStory;

    public GameObject StoryBase;
    [SerializeField]
    private List<SpecialStoryObjects> SpecialStoryObjects;
    [SerializeField]
    private Transform SpecialStoryObjectsTransform;
    [SerializeField]
    private GameObject nextArrow;

    [Space(10f)]
    [SerializeField]
    private TMP_Text nameBox;
    [SerializeField]
    private TMP_Text textBox;


    private int currCode = 0;
    private int lastStoryObjectCode = 0;
    private int lastStoryObjectIndex = 0;
    private bool isTextTypeEnd = true;
    private StringTable st;
    public void CheckNeedTutorial()
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        if (GamePlayerInfo.instance.storyQueue.Count > 0)
        {
            int currCode = GamePlayerInfo.instance.storyQueue.Peek();
            if (tutorialCodeDefines.Contains(currCode))
            {
                StartTutorial(currCode);
            }
        }

    }

    public void StartTutorial(int code)
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        switch (code)
        {
            case 0:
                SoundPlayer.instance.EnterLobbyMusic(1);
                break;
            default:
                break;
        }

        currentStory = storyDataDefines.Find(a => a.code == code);
        currCode = currentStory.startCode;
        if (SpecialStoryObjects.Count > 0 && currentStory.endCode >= SpecialStoryObjects[0].ID)
        {
            lastStoryObjectCode = 0;
            lastStoryObjectIndex = -1;
            int index = 0;
            foreach (var item in SpecialStoryObjects)
            {
                if (item.ID > currentStory.startCode)
                {
                    lastStoryObjectCode = item.ID;
                    lastStoryObjectIndex = index;
                    break;
                }
                index++;
            }
        }
        else
        {
            lastStoryObjectCode = 0;
            lastStoryObjectIndex = -1;
        }

        StoryBase.SetActive(true);
        UpdateStory(currCode);
    }

    public void UpdateStory(int code)
    {
        if (st == null)
        {
            st = DataTableManager.instance.Get<StringTable>(DataType.String);
        }
        currCode = code;
        nextArrow.SetActive(false);
        isTextTypeEnd = false;
        var currText = st.GetStory(currCode);
        nameBox.text = currText.Item1;
        textBox.text = "";
        textBox.DOText(currText.Item2, textSpeed * currText.Item2.Length).OnComplete(() =>
        {
            nextArrow.SetActive(true);
            isTextTypeEnd = true;
        });
    }

    public void FlipNextStory()
    {
        if (!isTextTypeEnd)
        {
            textBox.DOComplete(true);
        }
        else if (currCode == currentStory.endCode)
        {
            EndStory();
        }
        else if (lastStoryObjectCode == currCode + 1)
        {
            MakeSpecialObject();
        }
        else
        {
            UpdateStory(++currCode);
        }
    }

    public void MakeSpecialObject()
    {
        if (lastStoryObjectIndex < 0)
        {
            return;
        }
        StoryBase.SetActive(false);
        Instantiate(SpecialStoryObjects[lastStoryObjectIndex].prefab, SpecialStoryObjectsTransform);
        if (SpecialStoryObjects.Count <= lastStoryObjectIndex + 1)
        {
           lastStoryObjectCode = 0;
            lastStoryObjectIndex = -1;
        }
        else
        {
            lastStoryObjectIndex++;
            lastStoryObjectCode = SpecialStoryObjects[lastStoryObjectIndex].ID;
        }
    }

    public void EndStory()
    {
        StoryBase.SetActive(false);
        GamePlayerInfo.instance.storyQueue.Dequeue();
        GamePlayerInfo.instance.SaveFile();
        CheckNeedTutorial();
    }

    
}
