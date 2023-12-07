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
public class RecruitUIManager : MonoBehaviour
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


    private void Awake()
    {
        int index = 0;
        foreach (var tog in recruitToggles)
        {
            int code = index++;
            tog.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    SetRecruitMode(code);
                }
            }
            );
        }
        SetRecruitMode(0);
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
