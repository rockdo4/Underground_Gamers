using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickAnyWhere : MonoBehaviour
{
    [SerializeField]
    private GameObject popupNameInput;
    [SerializeField]
    private TMP_InputField nameInputBox;

    private void Update()
    {
    }

    public void LoadingScene()
    {
        if(GamePlayerInfo.instance.isInit)
        {
            nameInputBox.text = DataTableManager.instance.Get<StringTable>(DataType.String).Get("default_name_value");
            popupNameInput.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Lobby Scene");
        }
    }

    public void EndNameInput()
    {
        GamePlayerInfo.instance.playername = nameInputBox.text;
        GamePlayerInfo.instance.tutorials = new Queue<int>();
        GamePlayerInfo.instance.AddTutorial(0);

       SceneManager.LoadScene("Lobby Scene");
    }
    
    public void InputStreamString(string newText)
    {
        if (newText.Length > 8)
        {
            nameInputBox.text = newText.Substring(0, 8);
        }
    }


}
