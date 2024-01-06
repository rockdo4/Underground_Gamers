using TMPro;
using UnityEngine;

public class StoryObj3_4 : MonoBehaviour
{
    private void Update()
    {
        if (LobbyUIManager.instance.sceneLoader.loadingUI.activeSelf)
        {
            StoryManager.instance.EndStory();
            Destroy(gameObject);
        }
    }
}
