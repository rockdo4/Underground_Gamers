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
            gameManager.IsStart = false;

            // 라인 지정이 끝난후 이 함수 호출
            {
                GameInfo.instance.MakePlayers();
                gameManager.settingAIID.SetAIIDs();
                gameManager.entryManager.ResetEntry();
                gameManager.gameRuleManager.SetGameType(GameInfo.instance.gameType);
            }
        }
    }
}
