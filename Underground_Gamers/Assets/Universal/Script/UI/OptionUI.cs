using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
