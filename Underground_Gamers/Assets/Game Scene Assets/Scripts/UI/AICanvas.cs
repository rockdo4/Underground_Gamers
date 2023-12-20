using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICanvas : MonoBehaviour
{
    public Slider reloadBar;
    public Slider hpBar;
    public Image classIcon;

    public void SetClassIcon(Sprite sprite)
    {
        classIcon.sprite = sprite;
    }
}
