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
            GameInfo.instance.StartGame();

            // 엔트리를 결정 후, 시작
            //GameInfo.instance.SetEntryPlayer();
            if(gameManager.gameRuleManager.gameType == GameType.Official)
            {
                gameManager.entryPanel.SetActiveEntryPanel(true);
                gameManager.entryPanel.SetPlayerEntrySlotAndBenchSlot();
            }
            else
            {
                gameManager.battleLayoutForge.SetActiveBattleLayoutForge(true);
            }


            // 엔트리 결정 후 라인 지정 가기전에 해줘야 할 것들
            //GameInfo.instance.MakePlayers();
            //gameManager.settingAIID.SetAIIDs();
            // 라인 지정 UI
            //gameManager.entryManager.ResetBattleLayoutForge();
        }
    }
}
