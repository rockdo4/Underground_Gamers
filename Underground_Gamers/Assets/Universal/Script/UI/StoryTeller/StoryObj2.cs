using TMPro;
using UnityEngine;

public class StoryObj2 : MonoBehaviour
{
    [SerializeField] 
    TMP_InputField nameBox;
    public void SetTeamName()
    {
        GamePlayerInfo.instance.teamName = nameBox.text;
        BaseUIManager.instance.UpdateProfile();
        StoryManager.instance.StoryBase.SetActive(true);
        StoryManager.instance.UpdateStory(1029);
        Destroy(gameObject);
    }
}
