using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragBattleLayoutGhostImage : MonoBehaviour
{
    public Image ghostImage;
    public Image frameImage;
    public Image classIcon;

    public void SetGhostImage(Sprite imageSprite, Sprite classSprite, Color color)
    {
        ghostImage.sprite = imageSprite;
        this.classIcon.sprite = classSprite;
        frameImage.color = color;
    }

    public void SetActiveGhostImage(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
