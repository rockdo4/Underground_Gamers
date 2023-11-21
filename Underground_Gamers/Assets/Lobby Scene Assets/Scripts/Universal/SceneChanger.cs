using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void ToGameScene()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public static void ToLobbyScene()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}
