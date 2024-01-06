using UnityEngine;
public class Test : MonoBehaviour
{
    bool init = true;
    private void Update()
    {
        if (GamePlayerInfo.instance.isInit)
        {
            for (int i = 0; i < 1; i++)
            {
                var pl1 = GamePlayerInfo.instance.AddPlayer(0);
                var pl2 = GamePlayerInfo.instance.AddPlayer(3);
                var pl3 = GamePlayerInfo.instance.AddPlayer(6);
                var pl4 = GamePlayerInfo.instance.AddPlayer(7);
                var pl5 = GamePlayerInfo.instance.AddPlayer(14);
            }
            init = false;
        }
       Destroy(gameObject);
    }

}
