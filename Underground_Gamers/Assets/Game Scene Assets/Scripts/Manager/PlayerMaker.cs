using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaker : MonoBehaviour
{
    public GameManager gameManager;
    private void Awake()
    {
        // 현재 이것은 Fix
        if (GameInfo.instance != null)
        {
            gameManager.IsStart = false;
            gameManager.gameRuleManager.SetGameType(GameInfo.instance.gameType);

            // 선수 후보 선택 만들때 적용, 엔트리 OK 버튼 누르면

            // 엔트리를 결정 후, 시작
            //GameInfo.instance.SetEntryPlayer();
            if(gameManager.gameRuleManager.gameType == GameType.Official)
            {
                gameManager.entryPanel.SetActiveEntryPanel(true);
                gameManager.entryPanel.SetOriginMemberIndex();
                gameManager.entryPanel.SetPlayerEntrySlotAndBenchSlot();
            }
            else
            {
                GameInfo.instance.StartGame();
                GameInfo.instance.MakePlayers();
                gameManager.settingAIID.SetAIIDs();
                gameManager.entryManager.ResetBattleLayoutForge();
                gameManager.battleLayoutForge.SetActiveBattleLayoutForge(true);
                gameManager.IsStart = false;
                Time.timeScale = 0f;
            }
        }
    }
}
