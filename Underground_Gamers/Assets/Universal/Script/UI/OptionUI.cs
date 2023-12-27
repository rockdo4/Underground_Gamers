using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
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
    public AudioMixer audioMixer;

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
    private Toggle[] muteToggles;
    [SerializeField]
    private Slider[] volumes;

    private int quality = 2;
    private int resolution = 2;
    private bool isPost_fx = true;
    private int textureQuality = 2;
    private bool isShadowOn = true;
    private bool is60FPS = true;
    private int language = 0;

    private bool isSkillIllust = true;
    private bool isSkillEffect = true;

    private bool muteMaster = false;
    private bool muteBackground = false;
    private bool muteEffect = false;

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
        GetInfoFromSave();
        SetButtons();
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

    public void InvokeOptionSettingLobby()
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
        InvokeGlobalSettings(isPost_fx);
        QualitySettings.globalTextureMipmapLimit = 2 - textureQuality;

        if (isShadowOn)
        {
            QualitySettings.shadows = ShadowQuality.All;
        }
        else
        {
            QualitySettings.shadows = ShadowQuality.Disable;
        }

        if(muteMaster)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(0.001f) * 20);
        }
        else
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volumeMaster) * 20);
        }
        if (muteBackground)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(0.001f) * 20);
        }
        else
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(volumeBackground) * 20);
        }
        if (muteEffect)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(0.001f) * 20);
        }
        else
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(volumeEffect) * 20);
        }

        if (language != GamePlayerInfo.instance.language) 
        {
            GamePlayerInfo.instance.language = language;
            DataTableManager.instance.Get<StringTable>(DataType.String).DataAdder();
            var texts = FindObjectsOfType<SetString>();
            foreach ( var text in texts ) 
            {
                text.ResetString();
            }
        }
        //인게임 이펙트/일러는 부탁드리겠습니다..
    }

    public void GetInfoFromSave()
    {
        quality = GamePlayerInfo.instance.quality;
        resolution = GamePlayerInfo.instance.resolution;
        isPost_fx = GamePlayerInfo.instance.isPost_fx;
        textureQuality = GamePlayerInfo.instance.textureQuality;
        isShadowOn = GamePlayerInfo.instance.isShadowOn;
        is60FPS = GamePlayerInfo.instance.is60FPS;
        language = GamePlayerInfo.instance.language;
        isSkillIllust = GamePlayerInfo.instance.isSkillIllust;
        isSkillEffect = GamePlayerInfo.instance.isSkillEffect;
        muteMaster = GamePlayerInfo.instance.muteMaster;
        muteBackground = GamePlayerInfo.instance.muteBackground;
        muteEffect = GamePlayerInfo.instance.muteEffect;
        volumeMaster = GamePlayerInfo.instance.volumeMaster;
        volumeBackground = GamePlayerInfo.instance.volumeBackground;
        volumeEffect = GamePlayerInfo.instance.volumeEffect;
    }

    public void RefreshOptionSettings()
    {
        GetInfoFromSave();
        InvokeOptionSettingLobby();
    }

    public void SetButtons()
    {

    }
}
