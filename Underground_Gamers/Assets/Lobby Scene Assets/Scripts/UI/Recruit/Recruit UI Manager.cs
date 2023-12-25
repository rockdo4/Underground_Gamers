using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum RecruitType
{
    Recruit,
    Tryout,
    Trade
}
public class RecruitUIManager : LobbySceneSubscriber
{
    public static RecruitUIManager instance
    {
        get
        {
            if (recruitUIManager == null)
            {
                recruitUIManager = FindObjectOfType<RecruitUIManager>();
            }
            return recruitUIManager;
        }
    }

    private static RecruitUIManager recruitUIManager;

    [SerializeField]
    private List<ManagerRecruit> recruitManagers = new List<ManagerRecruit>();
    [SerializeField]
    private List<Toggle> recruitToggles = new List<Toggle>();

    [SerializeField]
    private Transform[] upperUITransforms = new Transform[3];
    private Transform originPos;

    protected override void Awake()
    {
        base.Awake();
        int index = 0;
        foreach (var tog in recruitToggles)
        {
            int code = index++;
            tog.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    SetRecruitMode(code);
                    lobbyTopMenu.transform.SetParent(upperUITransforms[code]);
                }
            }
            );
        }
        SetRecruitMode(0);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
        lobbyTopMenu.AddFunction(OnBack);
        originPos = lobbyTopMenu.transform.parent;
        lobbyTopMenu.transform.SetParent(upperUITransforms[0]);
        StartRecruit();
    }

    public override void OnExit()
    {
        base.OnExit();
        LobbyUIManager.instance.ActiveLobby(true);
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(false);
        lobbyTopMenu.transform.SetParent(originPos);
    }

    public void OnBack()
    {
        lobbySceneUIManager.OpenWindow(1);
    }

    public void SetRecruitMode(int code)
    {
        if (code > 2)
        {
            Debug.Log("Not Allowd Recruit Code");
            return;
        }
        for (int i = 0; i < recruitManagers.Count; i++)
        {
            if (i == code)
            {
                recruitManagers[i].OnEnter();
            }
            else
            {
                recruitManagers[i].OnExit();
            }
        }
    }

    public void StartRecruit()
    {
        recruitToggles[0].isOn = true;
    }
}
