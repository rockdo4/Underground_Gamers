using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandInfo : MonoBehaviour
{
    public string aiType;
    public int aiNum;

    public GameObject respawnTimeUI;
    public TextMeshProUGUI respawnTimeText;
    public AIController aiController;

    public Transform respawner;

    private void Start()
    {
        respawnTimeUI.SetActive(false);
    }

    public void OnRespawnUI(float time)
    {
        respawnTimeText.text = $"{Mathf.RoundToInt(time)}";
        respawnTimeUI.SetActive(true);
    }

    public void OffRespawnUI()
    {
        respawnTimeUI.SetActive(false);
    }

    public void DisplayRespawnTime(float time)
    {
        respawnTimeText.text = $"{Mathf.RoundToInt(time)}";
    }
}