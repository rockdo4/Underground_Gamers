using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTopMenu : MonoBehaviour
{
    public TMP_Text TabNameText;
    public TMP_Text[] moneyText;
    public Button backButton;
    public Button homeButton;
    public Button gameStartButton;
    public Button officialGameStartButton;
    public Button scrimmageGameStartButton;
    public Stack<Action> functionStack = new Stack<Action>();


    public void ActiveTop(bool on)
    {
        gameObject.SetActive(on);
    }
    public void UpdateMoney()
    {
        moneyText[0].text = GamePlayerInfo.instance.money.ToString("N0");
        moneyText[1].text = GamePlayerInfo.instance.crystal.ToString("N0");
    }

    public void AddFunction(Action action)
    {
        functionStack.Push(action);
    }

    public void ExecuteFunction()
    {
        if (functionStack.Count == 0)
        {
            LobbySceneUIManager.instance.EmergencyOut();
            //Debug.Log("Execute Nothing!");
            return;
        }
        Action function = functionStack.Pop();
        function.Invoke();
    }
    public void ExecuteFunction(int count)
    {
        if (functionStack.Count == 0)
        {
            LobbySceneUIManager.instance.EmergencyOut();
            //Debug.Log("Execute Nothing!");
            return;
        }
        int executedCount = 0;

        while (functionStack.Count > 0 && executedCount < count)
        {
            Action function = functionStack.Pop();
            function.Invoke();
            executedCount++;
        }
    }

    public void ExecuteAllFunction()
    {
        if (functionStack.Count == 0)
        {
            LobbySceneUIManager.instance.EmergencyOut();
            //Debug.Log("Execute Nothing!");
            return;
        }

        while (functionStack.Count > 0)
        {
            Action function = functionStack.Pop();
            function.Invoke();
        }
    }

    public void DeleteAllFunction()
    {
        functionStack.Clear();
    }

    public void DeleteOneFunction()
    {
        if (functionStack.Count == 0)
        {
            LobbySceneUIManager.instance.EmergencyOut();
            //Debug.Log("Delete Nothing!");
            return;
        }
        functionStack.Pop();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && functionStack.Count > 0)
        {
            ExecuteFunction();
        }
    }
}
