using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaker : MonoBehaviour
{
    public GameManager gameManager;
    private void Awake()
    {
        if (GameInfo.instance != null)
        {
            GameInfo.instance.MakePlayers();
            gameManager.settingAIID.SetAIIDs();
            gameManager.entryManager.SetEntry();
        }
    }
}
