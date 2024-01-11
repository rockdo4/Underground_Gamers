using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject extraButton;
    private void Start()
    {
        if (GamePlayerInfo.instance.cleardStage >= 406)
        {
            extraButton.SetActive(true);
        }
        else
        {
            extraButton.SetActive(false);
        }
    }
}
