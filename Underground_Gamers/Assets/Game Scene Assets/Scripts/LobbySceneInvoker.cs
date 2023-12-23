using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyOpenFunc
{
    None,
    Schedule,
}
public class LobbySceneInvoker : MonoBehaviour
{
    [SerializeField]
   public void OpenLobbySceneMenus(int funcCode)
    {
        switch ((LobbyOpenFunc)funcCode)
        {
            case LobbyOpenFunc.Schedule:
                GamePlayerInfo.instance.willOpenMenu = (int)GameInfo.instance.gameType + 1;
                break;
            default:
                break;
        }
    }
}
