using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaker : MonoBehaviour
{
    private void Awake()
    {
        if (GameInfo.instance != null)
        {
            GameInfo.instance.MakePlayers();
        }
    }
}
