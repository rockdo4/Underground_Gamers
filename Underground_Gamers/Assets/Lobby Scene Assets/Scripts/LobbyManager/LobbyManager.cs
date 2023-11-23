using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance
    {
        get
        {
            if (lobbyManager == null)
            {
                lobbyManager = FindObjectOfType<LobbyManager>();
            }
            return lobbyManager;
        }
    }

    private static LobbyManager lobbyManager;

}
