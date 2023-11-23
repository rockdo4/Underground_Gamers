using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{ 
    private void Awake()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Player newPlayer = new Player();
            newPlayer.code = 0;
            GamePlayerInfo.instance.havePlayers.Add(newPlayer);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Player newPlayer = new Player();
            newPlayer.code = 1;
            GamePlayerInfo.instance.havePlayers.Add(newPlayer);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player newPlayer = new Player();
            newPlayer.code = 2;
            GamePlayerInfo.instance.havePlayers.Add(newPlayer);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Player newPlayer = new Player();
            newPlayer.code = 3;
            GamePlayerInfo.instance.havePlayers.Add(newPlayer);
        }
    }
}
