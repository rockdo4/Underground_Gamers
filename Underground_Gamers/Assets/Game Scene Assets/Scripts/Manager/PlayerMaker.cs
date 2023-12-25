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
            gameManager.IsStart = false;
            gameManager.gameRuleManager.SetGameType(GameInfo.instance.gameType);

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


            // 경기 이름
            gameManager.stageInfoManager.InitString();
            gameManager.stageInfoManager.SetStageInfo();
        }
    }
}
