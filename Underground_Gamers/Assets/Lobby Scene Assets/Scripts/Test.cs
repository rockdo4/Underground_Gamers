using UnityEngine;
public class Test : MonoBehaviour
{
    bool init = true;
    private void Update()
    {
        if (init)
        {
            for (int i = 0; i < 50; i++)
            {
                var pl1 = GamePlayerInfo.instance.AddPlayer(0);
                pl1.xp = 1;
                pl1.level = 4;
                var pl2 = GamePlayerInfo.instance.AddPlayer(1);
                pl2.xp = 3;
                pl2.level = 3;
                var pl3 = GamePlayerInfo.instance.AddPlayer(2);
                pl3.xp = 5;
                pl3.level = 2;
                var pl4 = GamePlayerInfo.instance.AddPlayer(3);
                pl4.xp = 7;
                pl4.level = 1;
            }

            for (int i = 0; i < GamePlayerInfo.instance.XpItem.Count; i++)
            {
                GamePlayerInfo.instance.XpItem[i] += 5;
            }
            GamePlayerInfo.instance.money += 30000;
            GamePlayerInfo.instance.contractTicket += 300;
            GamePlayerInfo.instance.crystal += 5000;
            init = false;
            LobbyUIManager.instance.UpdateMoneyInfo();
            Destroy(gameObject);
        }
    }
}
