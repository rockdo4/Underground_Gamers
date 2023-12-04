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
    public float ID;
    public List<GameObject> releaseEffects;
    public bool willRelease = false;
    public void Awake()
    {
        willRelease = false;
    }
    public void SetImage(Sprite spr)
    {
        image.sprite = spr;
    }
    public void SetReleaseSelect(bool on)
    {
        foreach (GameObject obj in releaseEffects) 
        {
            obj.SetActive(on);
            willRelease = on;
        }
    }
    public void SetReleaseSelect()
    {
        willRelease = !willRelease;
        foreach (GameObject obj in releaseEffects)
        {
            obj.SetActive(willRelease);
        }
    }
}
