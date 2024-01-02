using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObj0 : MonoBehaviour
{
    [SerializeField]
    private GameObject nextPrefab;
    private void Update()
    {
        if (LobbySceneUIManager.instance.currUIIndex == 4)
        {
            Instantiate(nextPrefab,transform.parent);
            StoryManager.instance.StoryBase.SetActive(true);
            StoryManager.instance.UpdateStory(1016);
            Destroy(gameObject);
        }
    }
}
