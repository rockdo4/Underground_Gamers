using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public EffectType effectType = EffectType.Option_Button;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => SoundPlayer.instance.PlayEffectSound((int)effectType));
    }
}
