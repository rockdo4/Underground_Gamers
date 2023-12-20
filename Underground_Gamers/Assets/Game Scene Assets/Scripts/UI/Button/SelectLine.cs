using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLine : MonoBehaviour
{
    public GameObject battleLayoutForgePanel;
    public void FixLine()
    {
        Time.timeScale = 1f;
        battleLayoutForgePanel.SetActive(false);
    }
}
