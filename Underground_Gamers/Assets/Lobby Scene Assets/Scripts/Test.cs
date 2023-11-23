using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GamePlayerInfo.instance.AddPlayer(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GamePlayerInfo.instance.AddPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GamePlayerInfo.instance.AddPlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GamePlayerInfo.instance.AddPlayer(3);
        }
    }
}
