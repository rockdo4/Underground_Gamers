using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPannel : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public TextMeshProUGUI LoseText;

    public void EnterLobby()
    {
        SceneManager.LoadScene("Lobby Scene");
    }

}
