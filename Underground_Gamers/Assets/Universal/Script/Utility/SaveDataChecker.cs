using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveDataChecker : MonoBehaviour
{
    [SerializeField]
    private GameObject saveDataObj;
    void Awake()
    {
        var target = GameObject.FindGameObjectWithTag("SaveData");
        if (target == null )
        {
            Instantiate(saveDataObj);
        }
    }
}
