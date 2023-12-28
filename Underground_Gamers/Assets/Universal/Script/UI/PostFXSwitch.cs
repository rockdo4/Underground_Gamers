using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PostFXSwitch : MonoBehaviour
{
    private PostProcessLayer postProcessLayer;

    private void Awake()
    {
        postProcessLayer = GetComponent<PostProcessLayer>();
    }
    private void OnEnable()
    {
        if (postProcessLayer == null)
        {
            postProcessLayer = GetComponent<PostProcessLayer>();
        }
        OptionUI.globalSettings += ActivePostProcess;
    }

    private void OnDisable()
    {
        // OnSceneLoaded 이벤트에서 OnSceneLoadedHandler 메서드를 제거
        OptionUI.globalSettings -= ActivePostProcess;
    }


    private void ActivePostProcess(bool on)
    {
        postProcessLayer.enabled = on;
    }
}
