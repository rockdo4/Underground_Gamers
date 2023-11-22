using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtons : MonoBehaviour
{
    public Image image;
    public int index;
    public void SetImage(Sprite spr)
    {
        image.sprite = spr;
    }
}
