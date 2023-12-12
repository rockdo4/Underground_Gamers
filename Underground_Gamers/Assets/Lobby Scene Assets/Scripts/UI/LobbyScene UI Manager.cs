using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyType
{
    None,
    Base,
    Recruit,
    Upgrade,
    Management,
    Schedule
}
public class LobbySceneUIManager : MonoBehaviour
{
    private Dictionary<LobbyType, LobbySceneSubscriber> lobbySceneSubscribers;
    [SerializeField]
    public LobbyTopMenu lobbyTopMenu;

    private int currUIIndex = 1;
    public void Subscribe(LobbySceneSubscriber lobbySceneSubscriber, LobbyType type)
    {
        if (lobbySceneSubscribers == null)
        {
            lobbySceneSubscribers = new Dictionary<LobbyType, LobbySceneSubscriber>();
        }
        lobbySceneSubscribers.Add(type,lobbySceneSubscriber);
    }

    public void OpenWindow(int type)
    {
        lobbySceneSubscribers[(LobbyType)currUIIndex].OnExit();
        currUIIndex = type;
        lobbySceneSubscribers[(LobbyType)type].gameObject.SetActive(true);
        lobbySceneSubscribers[(LobbyType)type].OnEnter();
        lobbyTopMenu.UpdateMoney();
    }
}
