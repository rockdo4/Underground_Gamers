using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObj1 : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private bool isFullSquad = false;
    void Update()
    {
       
        if (LobbyUIManager.instance.playerSlotSet.activeSelf)
        {
            target.SetActive(false);
        }
        else
        {
            target.SetActive(true);
        }

        isFullSquad = true;
        for (int i = 0; i < 5; i++)
        {
            if (GamePlayerInfo.instance.usingPlayers[i].code < 0)
            {
                isFullSquad = false;
            }
           
        }
        if (isFullSquad)
        {
            StoryManager.instance.StoryBase.SetActive(true);
            StoryManager.instance.UpdateStory(1022);
            Destroy(gameObject);
        }
    }
}
