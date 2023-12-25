using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListManager : LobbySceneSubscriber
{
    public static PlayerListManager instance
    {
        get
        {
            if (playerListManager == null)
            {
                playerListManager = FindObjectOfType<PlayerListManager>();
            }
            return playerListManager;
        }
    }

    private static PlayerListManager playerListManager;

    [SerializeField]
    private Transform lobbyTopMenuTransform;
    [SerializeField]
    private GameObject releasePanel;
    [SerializeField]
    private Transform originPos;
    private PlayersFilter playersFilter;



    protected override void Awake()
    {
        base.Awake();
        playersFilter = lobbySceneUIManager.GetComponent<PlayersFilter>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
        LobbyUIManager.instance.ActiveLobby(false);
        playersFilter.ResetSortStandard();
        playersFilter.ResetToggleList();
        PlayerChanger.instance.OpenPlayers();
        playersFilter.Filtering();
        lobbyTopMenu.AddFunction(OnBack);
        originPos = lobbyTopMenu.transform.parent;
        lobbyTopMenu.transform.SetParent(lobbyTopMenuTransform);
    }

    public override void OnExit()
    {
        LobbyUIManager.instance.ActiveLobby(true);
        LobbyUIManager.instance.ActivePlayerListAnyway(false);
        PlayerReleaser.instance.EndReleaseMod();
        releasePanel.SetActive(false);
        base.OnExit();
        lobbyTopMenu.transform.SetParent(originPos);
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(false);
    }

    public void OnBack()
    {
        lobbySceneUIManager.OpenWindow(1);
    }

    public void StartRelease()
    {
        releasePanel.SetActive(true);
        PlayerReleaser.instance.StartReleaseMod();
        lobbyTopMenu.DeleteAllFunction();
        lobbyTopMenu.AddFunction(OnBack);
    }
}
