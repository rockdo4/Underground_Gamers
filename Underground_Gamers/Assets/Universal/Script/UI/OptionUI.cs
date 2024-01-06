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
    private GameObject popupOptionCancelCheck;
    [SerializeField]
    private Toggle[] toggleQuality;
    [SerializeField]
    private Toggle[] toggleResolution;
    [SerializeField]
    private Toggle[] toggleTextureQuality;
    [SerializeField]
    private Toggle[] togglePost_FX;
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
    private int textureQuality = 2;
    private bool isPost_fx = true;
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

    private bool isChanged = false;

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

        for (int i = 0; i < toggleResolution.Length; i++)
        {
            int index = i;
            toggleResolution[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    resolution = index;
                    isChanged = true;
                    toggleQuality[5].isOn = true;
                    CheckPreset();
                }
            });
        }

        for (int i = 0; i < toggleTextureQuality.Length; i++)
        {
            int index = i;
            toggleTextureQuality[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    textureQuality = index;
                    isChanged = true;
                    toggleQuality[5].isOn = true;
                    CheckPreset();
                }
            });
        }

        for (int i = 0; i < togglePost_FX.Length; i++)
        {
            int index = i;
            togglePost_FX[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    isPost_fx = index == 0;
                    isChanged = true;
                    toggleQuality[5].isOn = true;
                    CheckPreset();
                }
            });
        }
        for (int i = 0; i < toggleShadow.Length; i++)
        {
            int index = i;
            toggleShadow[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    isShadowOn = index == 0;
                    isChanged = true;
                    toggleQuality[5].isOn = true;
                    CheckPreset();
                }
            });
        }
        for (int i = 0; i < toggle60FPS.Length; i++)
        {
            int index = i;
            toggle60FPS[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    is60FPS = index == 1;
                    isChanged = true;
                    toggleQuality[5].isOn = true;
                    CheckPreset();
                }
            });
        }
        for (int i = 0; i < toggleSkillIllust.Length; i++)
        {
            int index = i;
            toggleSkillIllust[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    isSkillIllust = index == 0;
                    isChanged = true;
                }
            });
        }
        for (int i = 0; i < toggleSkillEffect.Length; i++)
        {
            int index = i;
            toggleSkillEffect[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    isSkillEffect = index == 0;
                    isChanged = true;
                }
            });
        }
        for (int i = 0; i < toggleLanguage.Length; i++)
        {
            int index = i;
            toggleLanguage[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    language = index;
                    isChanged = true;
                }
            });
        }
        for (int i = 0; i < muteToggles.Length; i++)
        {
            int index = i;
            muteToggles[i].onValueChanged.AddListener(value =>
            {
                switch (index)
                {
                    case 0:
                        muteMaster = value;
                        break;
                    case 1:
                        muteBackground = value;
                        break;
                    default:
                        muteEffect = value;
                        break;
                }
                isChanged = true;
            });
        }

        for (int i = 0; i < toggleQuality.Length - 1; i++)
        {
            int index = i;
            toggleQuality[i].onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    SetGraphicsPresets(index);
                    quality = index;
                    isChanged = true;
                }
            });
        }

        for (int i = 0; i < volumes.Length; i++)
        {
            int index = i;
            volumes[i].onValueChanged.AddListener(value =>
            {
                switch (index)
                {
                    case 0:
                        volumeMaster = value;
                        break;
                    case 1:
                        volumeBackground = value;
                        break;
                    default:
                        volumeEffect = value;
                        break;
                }
                quality = index;
                isChanged = true;
            });
        }
    }
    public void OpenOption()
    {
        popupOption.SetActive(true);
        GetInfoFromSave();
        SetButtons();
        isChanged = false;
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
        var filePath = Path.Combine(Application.persistentDataPath, "534789fd.json");
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
                resolutionValue = 0.5f;
                break;
            case 1:
                resolutionValue = 1f;
                break;
            case 2:
                resolutionValue = 1.2f;
                break;
            case 3:
                resolutionValue = 1.5f;
                break;
        }

        Application.targetFrameRate = is60FPS switch
        {
            true => 60,
            false => 30,
        };
        Screen.SetResolution((int)(originWidth * resolutionValue), (int)(originHeight * resolutionValue), true);
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

        if (muteMaster)
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
            var texts = FindObjectsOfType<SetString>();
            foreach (var text in texts)
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
        toggleResolution[resolution].isOn = true;
        toggleTextureQuality[textureQuality].isOn = true;
        togglePost_FX[isPost_fx ? 0 : 1].isOn = true;
        toggleShadow[isShadowOn ? 0 : 1].isOn = true;
        toggle60FPS[is60FPS ? 1 : 0].isOn = true;

        muteToggles[0].isOn = muteMaster;
        muteToggles[1].isOn = muteBackground;
        muteToggles[2].isOn = muteEffect;

        volumes[0].value = volumeMaster;
        volumes[1].value = volumeBackground;
        volumes[2].value = volumeEffect;

        toggleSkillIllust[isSkillIllust ? 0 : 1].isOn = true;
        toggleSkillEffect[isSkillEffect ? 0 : 1].isOn = true;

        toggleLanguage[language].isOn = true;
        quality = GamePlayerInfo.instance.quality;
        toggleQuality[quality].isOn = true;
    }

    public void SetResolution(int value)
    {
        resolution = value;
    }

    public void SetGraphicsPresets(int num)
    {
        switch (num)
        {
            case 0:
                {
                    toggleResolution[0].isOn = true;
                    resolution = 0;
                    toggleTextureQuality[0].isOn = true;
                    textureQuality = 0;
                    togglePost_FX[1].isOn = true;
                    isPost_fx = false;
                    toggleShadow[1].isOn = true;
                    isShadowOn = false;
                    toggle60FPS[0].isOn = true;
                    is60FPS = false;

                    toggleQuality[0].isOn = true;
                    quality = 0;
                }
                break;
            case 1:
                {
                    toggleResolution[1].isOn = true;
                    resolution = 1;
                    toggleTextureQuality[1].isOn = true;
                    textureQuality = 1;
                    togglePost_FX[1].isOn = true;
                    isPost_fx = false;
                    toggleShadow[1].isOn = true;
                    isShadowOn = false;
                    toggle60FPS[0].isOn = true;
                    is60FPS = false;

                    toggleQuality[1].isOn = true;
                    quality = 1;
                }
                break;
            case 2:
                {
                    toggleResolution[1].isOn = true;
                    resolution = 1;
                    toggleTextureQuality[1].isOn = true;
                    textureQuality = 1;
                    togglePost_FX[0].isOn = true;
                    isPost_fx = true;
                    toggleShadow[0].isOn = true;
                    isShadowOn = true;
                    toggle60FPS[0].isOn = true;
                    is60FPS = false;

                    toggleQuality[2].isOn = true;
                    quality = 2;
                }
                break;
            case 3:
                {
                    toggleResolution[2].isOn = true;
                    resolution = 2;
                    toggleTextureQuality[2].isOn = true;
                    textureQuality = 2;
                    togglePost_FX[0].isOn = true;
                    isPost_fx = true;
                    toggleShadow[0].isOn = true;
                    isShadowOn = true;
                    toggle60FPS[1].isOn = true;
                    is60FPS = true;

                    toggleQuality[3].isOn = true;
                    quality = 3;
                }
                break;
            case 4:
                {
                    toggleResolution[3].isOn = true;
                    resolution = 3;
                    toggleTextureQuality[2].isOn = true;
                    textureQuality = 2;
                    togglePost_FX[0].isOn = true;
                    isPost_fx = true;
                    toggleShadow[0].isOn = true;
                    isShadowOn = true;
                    toggle60FPS[1].isOn = true;
                    is60FPS = true;

                    toggleQuality[4].isOn = true;
                    quality = 4;
                }
                break;
            case 5:
                {
                    toggleQuality[5].isOn = true;
                    quality = 5;
                }
                break;
            default:
                break;
        }


    }

    public void CheckPreset()
    {
        if (resolution == 0 &&
            textureQuality == 0 &&
            isPost_fx == false &&
            isShadowOn == false &&
            is60FPS == false)
        {
            toggleQuality[0].isOn = true;
            quality = 0;
        }
        else if (resolution == 1 &&
            textureQuality == 1 &&
            isPost_fx == false &&
            isShadowOn == false &&
            is60FPS == false)
        {
            toggleQuality[1].isOn = true;
            quality = 1;
        }
        else if (resolution == 1 &&
                textureQuality == 1 &&
                isPost_fx == true &&
                isShadowOn == true &&
                is60FPS == false)
        {
            toggleQuality[2].isOn = true;
            quality = 2;
        }
        else if (resolution == 2 &&
                textureQuality == 2 &&
                isPost_fx == true &&
                isShadowOn == true &&
                is60FPS == true)
        {
            toggleQuality[3].isOn = true;
            quality = 3;
        }
        else if (resolution == 3 &&
                textureQuality == 2 &&
                isPost_fx == true &&
                isShadowOn == true &&
                is60FPS == true)
        {
            toggleQuality[4].isOn = true;
            quality = 4;
        }
        else
        {
            toggleQuality[5].isOn = true;
            quality = 5;
        }
    }

    public void SaveOptions()
    {
        GamePlayerInfo.instance.quality = quality;
        GamePlayerInfo.instance.resolution = resolution;
        GamePlayerInfo.instance.isPost_fx = isPost_fx;
        GamePlayerInfo.instance.textureQuality = textureQuality;
        GamePlayerInfo.instance.isShadowOn = isShadowOn;
        GamePlayerInfo.instance.is60FPS = is60FPS;
        GamePlayerInfo.instance.isSkillIllust = isSkillIllust;
        GamePlayerInfo.instance.isSkillEffect = isSkillEffect;
        GamePlayerInfo.instance.muteMaster = muteMaster;
        GamePlayerInfo.instance.muteBackground = muteBackground;
        GamePlayerInfo.instance.muteEffect = muteEffect;
        GamePlayerInfo.instance.volumeMaster = volumeMaster;
        GamePlayerInfo.instance.volumeBackground = volumeBackground;
        GamePlayerInfo.instance.volumeEffect = volumeEffect;
        GamePlayerInfo.instance.SaveFile();
    }

    public void DoOptionChange()
    {
        SaveOptions();
        InvokeOptionSettingLobby();
        popupOption.SetActive(false);
    }

    public void CancelOption()
    {
        if (isChanged)
        {
            popupOptionCancelCheck.SetActive(true);
        }
        else
        {
            popupOption.SetActive(false);
        }
    }
}
