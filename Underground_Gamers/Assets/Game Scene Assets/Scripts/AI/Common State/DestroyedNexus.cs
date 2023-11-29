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

    public void DestoryObject()
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
        }
        if (gameManager != null)
        {
            gameManager.IsPlaying = false;
            gameManager.timer = Time.time;
        }
    }
}
