using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementUIManager : LobbySceneSubscriber
{
    [SerializeField]
    private GameObject relesasePanel;
    [SerializeField]
    private Transform lobbyTopMenuTransform;
    [SerializeField]
    private Transform originPos;
    public override void OnEnter()
    {
        base.OnEnter();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
        LobbyUIManager.instance.ActivePlayerList(true);
        LobbyUIManager.instance.ActiveLobby(false);
        lobbyTopMenu.AddFunction(OnBack);
        originPos = lobbyTopMenu.transform.parent;
        lobbyTopMenu.transform.SetParent(lobbyTopMenuTransform);
    }

    public override void OnExit()
    {
        LobbyUIManager.instance.ActiveLobby(true);
        LobbyUIManager.instance.ActivePlayerListAnyway(false);
        relesasePanel.SetActive(false);
        PlayerReleaser.instance.EndReleaseMod();
        base.OnExit();
        lobbyTopMenu.transform.SetParent(originPos);
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(false);
    }

    public void ReAddOnBack()
    {
        lobbyTopMenu.AddFunction(OnBack);
    }

    public void OnBack()
    {
        if (LobbyUIManager.instance.ActivePlayerList(false))
        {
            lobbySceneUIManager.OpenWindow(1);
        }
    }
}
