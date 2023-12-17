using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPannel : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI LoseText;

    public GameObject rewardPanel;
    public GameObject damageGraphPanel;

    public Transform rewardParent;
    public Transform damageGraphParent;

    public AIReward rewardPrefab;
    public DamageGraph damageGraphPrefab;

    private float maxDealtDamage = 0;
    private float maxTakenDamage = 0;

    public void OnGameEndPanel()
    {
        gameObject.SetActive(true);
        OnRewardPanel();
        OffDamageGraph();
        CreateAIReward();
        CreateDamageGraph();
    }

    public void CreateAIReward()
    {
        foreach(var pc in gameManager.aiManager.pc)
        {
            AIReward aiReward = Instantiate(rewardPrefab, rewardParent);
            //이름
            aiReward.aiNameText.text = $"{pc.status.AIName}";
            aiReward.lvText.text = $"Lv. {pc.status.lv}";
            aiReward.ai = pc.status.ai;
            aiReward.illustration.sprite = pc.status.illustration;
            aiReward.aiClass.sprite = pc.status.aiClass;
            GameObject ai = Instantiate(aiReward.ai, aiReward.aiParent);
            ai.transform.position = aiReward.aiPos.position;
            ai.layer = LayerMask.NameToLayer("OverUI");
        }
    } 
        
    
    public void CreateDamageGraph()
    {
        foreach (var pc in gameManager.aiManager.pc)
        {
            DamageGraph damageGraph = Instantiate(damageGraphPrefab, damageGraphParent);
            // 여기서 이미지 받기
            pc.damageGraph = damageGraph;
            damageGraph.illustration.sprite = pc.status.illustration;
            damageGraph.dealtDamage = pc.status.dealtDamage;
            damageGraph.takenDamage = pc.status.takenDamage;

            if(maxDealtDamage < damageGraph.dealtDamage)
            {
                maxDealtDamage = damageGraph.dealtDamage;
            }            
            
            if(maxTakenDamage < damageGraph.takenDamage)
            {
                maxTakenDamage = damageGraph.takenDamage;
            }
        }

        foreach(var pc in gameManager.aiManager.pc)
        {
            pc.damageGraph.SetMaxDamages(maxDealtDamage, maxTakenDamage);
            pc.damageGraph.DisplayDamges();
        }
    }

    public void OffGameEndPanel()
    {
        gameObject.SetActive(false);
    }

    public void EnterLobby()
    {
        SceneManager.LoadScene("Lobby Scene");
    }

    public void OnDamageGraph()
    {
        damageGraphPanel.SetActive(true);
    }

    public void OnRewardPanel()
    {
        rewardPanel.SetActive(true);
    }
    public void OffDamageGraph()
    {
        damageGraphPanel.SetActive(false);
    }

    public void OffRewardPanel()
    {
        rewardPanel.SetActive(false);
    }
}
