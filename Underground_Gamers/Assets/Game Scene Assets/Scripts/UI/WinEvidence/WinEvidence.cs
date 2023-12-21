using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEvidence : MonoBehaviour
{
    [SerializeField]
    private GameObject fillEvidence;

    public void SetActiveWinEvidence(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void FillWinEvidence(bool isActive)
    {
        fillEvidence.SetActive(isActive);
    }
}
