using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GiveUp : MonoBehaviour
{
    public void OnGiveUp()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Lobby Scene");
    }
}
