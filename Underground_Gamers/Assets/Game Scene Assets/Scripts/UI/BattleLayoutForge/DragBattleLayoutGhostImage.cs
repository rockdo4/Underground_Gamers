using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragBattleLayoutGhostImage : MonoBehaviour
{
    public Image ghostImage; 

    public void SetGhostImage(Sprite imageSprite)
    {
        ghostImage.sprite = imageSprite;
    }

    public void SetActiveGhostImage(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
