using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestB : MonoBehaviour
{
    public TMP_Text testT;
    
    public void SetVal99()
    {
        SquadManager.playerCode = 99;
    }
    public void SetVal77()
    {
        SquadManager.playerCode = 77;
    }

    public void ShowVal()
    {
        testT.SetText(SquadManager.playerCode.ToString());
    }
}
