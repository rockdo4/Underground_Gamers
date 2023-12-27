using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public void ShowMeTheMoney()
    {
        GamePlayerInfo.instance.AddMoney(100000, 100000, 1000);
        if (SceneManager.GetActiveScene().name == "Lobby Scene")
        {
            LobbyUIManager.instance.UpdateMoneyInfo();
            LobbySceneUIManager.instance.lobbyTopMenu.UpdateMoney();
        }
    }
    public void ShowMeTheXp()
    {
        GamePlayerInfo.instance.GetXpItems(100, 100, 100,100);
    }

    public void BreakAllMaps()
    {
        GamePlayerInfo.instance.cleardStage = 405;
    }

    public void ResrtAllMaps()
    {
        GamePlayerInfo.instance.cleardStage = 0;
    }
}
