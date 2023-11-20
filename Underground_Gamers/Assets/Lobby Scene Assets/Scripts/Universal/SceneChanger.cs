using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ToGameScene()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void ToLobbyScene()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}
