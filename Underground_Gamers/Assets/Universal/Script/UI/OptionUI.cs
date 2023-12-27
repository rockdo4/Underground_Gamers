using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class OptionUI : MonoBehaviour
{
    public static OptionUI instance
    {
        get
        {
            if (optionUI == null)
            {
                optionUI = FindObjectOfType<OptionUI>();
            }
            return optionUI;
        }
    }
   
    private static OptionUI optionUI;

    [SerializeField]
    private GameObject popupOption;
    [SerializeField]
    private Toggle[] toggleQuality;
    [SerializeField]
    private Toggle[] toggleResolution;
    [SerializeField]
    private Toggle[] togglePost_FX;
    [SerializeField]
    private Toggle[] toggleTextureQuality;
    [SerializeField]
    private Toggle[] toggleShadow;
    [SerializeField]
    private Toggle[] toggle60FPS;
    [SerializeField]
    private Toggle[] toggleSkillIllust;
    [SerializeField]
    private Toggle[] toggleSkillEffect;
    [SerializeField]
    private Toggle[] toggleLanguage;
    [SerializeField]
    private Slider[] volumes;


    private int resolution = 2;
    private int post_fx = 2;
    private int textureQuality = 2;
    private bool isShadowOn = true;
    private bool is60FPS = true;

    private bool isSkillIllust = true;
    private bool isSkillEffect = true;

    private float volumeMaster = 1f;
    private float volumeBackground = 1f;
    private float volumeEffect = 1f;

    private float originWidth;
    private float originHeight;

    public delegate void GlobalSettings(bool on);
    public static event GlobalSettings globalSettings;
    public static void InvokeGlobalSettings(bool isOn)
    {
        globalSettings?.Invoke(isOn);
    }

    public void Awake()
    {
        originWidth = Screen.width;
        originHeight = Screen.height;
    }
    public void OpenOption()
    {
        popupOption.SetActive(true);
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        GamePlayerInfo.instance.SaveFile();
    }

    public void ResetGame()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        if (File.Exists(filePath))
        {
                File.Delete(filePath);
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void InvokeOptionSetting()
    {
        float resolutionValue = 0f;
        switch (resolution)
        {
            case 0:
                resolutionValue = 0.7f;
                break;
            case 1:
                resolutionValue = 1f;
                break;
            case 2:
                resolutionValue = 1.5f;
                break;
            case 3:
                resolutionValue = 2f;
                break;
        }

        Application.targetFrameRate = is60FPS switch
        {
            true => 60,
            false => 30,
        };
        Screen.SetResolution((int)(originWidth * resolutionValue), (int)(originHeight * resolutionValue),true);
        //InvokeGlobalSettings();
    }
}
