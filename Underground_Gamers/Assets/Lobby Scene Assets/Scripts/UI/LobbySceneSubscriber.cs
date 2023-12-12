using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneSubscriber : MonoBehaviour
{
    [SerializeField]
    public LobbySceneUIManager lobbySceneUIManager;
    [HideInInspector]
    public LobbyTopMenu lobbyTopMenu;
    [SerializeField]
    public LobbyType lobbyType;

    protected virtual void Awake()
    {
        lobbySceneUIManager.Subscribe(this, lobbyType);
    }
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public virtual void OnExit()
    {
        gameObject.SetActive(false);
    }
}
