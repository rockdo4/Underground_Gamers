using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainType
{
    Analyze,
    Train,
    Break
}
public class TrainingUIManager : LobbySceneSubscriber
{
    public static TrainingUIManager instance
    {
        get
        {
            if (trainingUIManager == null)
            {
                trainingUIManager = FindObjectOfType<TrainingUIManager>();
            }
            return trainingUIManager;
        }
    }

    private static TrainingUIManager trainingUIManager;
    [SerializeField]
    private Transform upperUITransform;
    [SerializeField]
    private Transform originPos;


    public List<ManagerTraining> trainingManagers = new List<ManagerTraining>();
    public override void OnEnter()
    {
        base.OnEnter();
        lobbyTopMenu.AddFunction(OnBack);
        lobbyTopMenu.transform.SetParent(upperUITransform);
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
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
    public void SetTraining(int code)
    {
        if (code > 2)
        {
            Debug.Log("Not Allowd Training Code");
            return;
        }
        for (int i = 0; i < trainingManagers.Count; i++) 
        {
            if (i == code)
            {
                trainingManagers[i].OnEnter();
            }
            else
            {
                trainingManagers[i].OnExit();
            }
        }
    }    

    public void AddFunctionBack()
    {
        lobbyTopMenu.AddFunction(FunctionBack);
    }

    public void FunctionBack()
    {
        foreach (var item in trainingManagers)
        {
            item.OnExit();
        }
        lobbyTopMenu.transform.SetParent(upperUITransform);
    }
}
