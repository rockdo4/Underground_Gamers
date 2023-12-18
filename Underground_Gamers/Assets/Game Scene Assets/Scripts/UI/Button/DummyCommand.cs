using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DummyCommand : MonoBehaviour
{
    public GameObject respawnTimeUI;
    public TextMeshProUGUI respawnTimeText;
    public Image respawnCoolTime;
    public GameObject aiSelectImage;
    public Image portrait;
    public TextMeshProUGUI aiName;
    public GameObject statusInfo;
    public Slider hpBar;
    public TextMeshProUGUI killCountText;
    public Image conditionIcon;
    public Image skillCoolTime;


    public void SetDummy(CommandInfo info)
    {
        respawnTimeUI = info.respawnTimeUI;
        respawnTimeText = info.respawnTimeText;
        respawnCoolTime = info.respawnCoolTime;
        aiSelectImage = info.aiSelectImage;
        portrait = info.portrait;
        aiName = info.aiName;
        statusInfo = info.statusInfo;
        hpBar = info.hpBar;
        killCountText = info.killCountText;
        conditionIcon = info.conditionIcon;
        skillCoolTime = info.skillCoolTime;
    }
}
