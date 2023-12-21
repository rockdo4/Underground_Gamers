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
        if (identity != null)
        {
            if (identity.teamLayer == LayerMask.GetMask("PC"))
            {
                gameManager.IsPlayerWin = false;
            }
            else
            {
                gameManager.IsPlayerWin = true;
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
