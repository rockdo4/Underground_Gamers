using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool init = true;
    private void Update()
    {
        if (init)
        {
            for (int i = 0; i < 4; i++)
            {
                var pl = GamePlayerInfo.instance.AddPlayer(0);
                pl.xp = 10;
                var pl2 = GamePlayerInfo.instance.AddPlayer(1);
                pl2.xp = 50;
                var pl3 = GamePlayerInfo.instance.AddPlayer(2);
                pl3.xp = 70;
                pl3.grade = 4;
                var pl4 = GamePlayerInfo.instance.AddPlayer(3);
                pl4.xp = 90;
                pl4.level = 1;
            }
            init = false;
        }
    }
}
