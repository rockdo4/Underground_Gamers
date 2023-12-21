using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaker : MonoBehaviour
{
    public GameManager gameManager;
    private void Awake()
    {
        if (GameInfo.instance != null)
        {
            // 선수 후보 선택 만들때 적용
            GameInfo.instance.StartGame();
            // 라인 지정이 끝난후 이 함수 호출
            {
                GameInfo.instance.MakePlayers();
                gameManager.settingAIID.SetAIIDs();
                gameManager.entryManager.InitEntry();
                gameManager.IsStart = false;
                gameManager.gameRuleManager.SetGameType(GameInfo.instance.gameType);
                gameManager.gameRuleManager.SetGameType(GameType.Official);
            }
        }
    }
}
