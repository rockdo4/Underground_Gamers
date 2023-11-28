using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtons : MonoBehaviour
{
    public Image image;
    public TMP_Text Level;
    public Image typeIcon;
    public Image stars;
    public Image isUsing;
    public TMP_Text playerNameCard;
    [HideInInspector]
    public int index;
    public void SetImage(Sprite spr)
    {
        image.sprite = spr;
    }
}
