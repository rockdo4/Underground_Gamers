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

    public void OnEnterWithStart()
    {
         base.OnEnter();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
        LobbyUIManager.instance.ActivePlayerList(true);
        lobbyTopMenu.AddFunction(BackToStage);
        originPos = lobbyTopMenu.transform.parent;
        lobbyTopMenu.transform.SetParent(lobbyTopMenuTransform);
        lobbyTopMenu.gameStartButton.gameObject.SetActive(true);
    }

    public void BackToStage()
    {
        LobbyUIManager.instance.ActivePlayerListAnyway(false);
        relesasePanel.SetActive(false);
        base.OnExit();
        lobbyTopMenu.transform.SetParent(originPos);
        lobbyTopMenu.TabNameText.text = DataTableManager.instance
            .Get<StringTable>(DataType.String).Get("select_schedule");
        lobbyTopMenu.gameStartButton.gameObject.SetActive(false);
    }

    public void OnEnterWithOfficialStart()
    {
        base.OnEnter();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
        LobbyUIManager.instance.ActivePlayerList(true);
        lobbyTopMenu.AddFunction(BackToOfficial);
        originPos = lobbyTopMenu.transform.parent;
        lobbyTopMenu.transform.SetParent(lobbyTopMenuTransform);
        lobbyTopMenu.officialGameStartButton.gameObject.SetActive(true);
    }

    public void BackToOfficial()
    {
        LobbyUIManager.instance.ActivePlayerListAnyway(false);
        relesasePanel.SetActive(false);
        base.OnExit();
        lobbyTopMenu.transform.SetParent(originPos);
        lobbyTopMenu.TabNameText.text = DataTableManager.instance
            .Get<StringTable>(DataType.String).Get("select_schedule");
        lobbyTopMenu.officialGameStartButton.gameObject.SetActive(false);
    }
}
