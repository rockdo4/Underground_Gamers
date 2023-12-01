using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public GameObject debugPannel;
    private bool isOn = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isOn = !isOn;
            debugPannel.SetActive(isOn);
        }
    }
}
