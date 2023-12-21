using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyedNexus : MonoBehaviour, IDestroyable
{
    private GameManager gameManager;
    public TeamIdentifier identity;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void DestoryObject(GameObject attacker)
    {
        if (gameManager.IsJudgement)
            return;
        if (identity != null)
        {
            if (identity.teamLayer == LayerMask.GetMask("PC"))
            {
                gameManager.IsRoundWin = false;
            }
            else
            {
                gameManager.IsRoundWin = true;
            }
            gameManager.GetWinner();
        }
        if (gameManager != null)
        {
            gameManager.IsPlaying = false;
            gameManager.endTimer = Time.time;
        }
    }
}
