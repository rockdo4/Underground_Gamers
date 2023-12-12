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

    public List<ManagerTraining> trainingManagers = new List<ManagerTraining>();
    public override void OnEnter()
    {
        base.OnEnter();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        lobbySceneUIManager.lobbyTopMenu.ActiveTop(false);
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
}
