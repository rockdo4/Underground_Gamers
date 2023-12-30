using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillLog : MonoBehaviour
{
    public KillLogPanel killLogPanel;

    public Transform killerPortraitParent;
    public Transform deadPortraitParent;
    public float scale;
    public float destroyedTimer;
    private float duration = 2f;

    public Color pcColor;
    public Color npcColor;

    private void Awake()
    {
        killLogPanel = GameObject.FindGameObjectWithTag("KillLogPanel").GetComponent<KillLogPanel>();
    }

    private void Update()
    {
        if(destroyedTimer + duration < Time.time)
        {
            killLogPanel.killLogs.Remove(this);
            Destroy(gameObject);
        }
    }

    public void SetKillLog(GameObject killerPortrait, GameObject deadPortrait, TeamIdentifier killerIdentity)
    {
        GameObject k_portrait = Instantiate(killerPortrait, killerPortraitParent);
        GameObject d_portrait = Instantiate(deadPortrait, deadPortraitParent);

        if(killerIdentity.teamLayer == LayerMask.GetMask("PC"))
        {
            killerPortraitParent.GetComponent<Image>().color = pcColor;
            deadPortraitParent.GetComponent<Image>().color = npcColor;
        }
        else
        {
            killerPortraitParent.GetComponent<Image>().color = npcColor;
            deadPortraitParent.GetComponent<Image>().color = pcColor;
        }

        //k_portrait.GetComponent<Image>().color =;
        k_portrait.SetActive(true);
        d_portrait.SetActive(true);
        SetPortrait(k_portrait, killerPortraitParent);
        SetPortrait(d_portrait, deadPortraitParent);
    }

    private void SetPortrait(GameObject portrait, Transform parent)
    {
        var towerPortrait = portrait.GetComponent<TowerKillLog>();
        if(towerPortrait != null)
        {
            portrait.transform.position = parent.position;
        }
        else
        {
            portrait.layer = LayerMask.NameToLayer("OverUI");
            portrait.transform.localScale = Vector3.one * scale;
            portrait.transform.position = parent.position;

            var portraitPos = portrait.transform.position;
            portraitPos.y = -26f;
            portraitPos.x = 3f;
            portraitPos.z = 0f;

            portrait.transform.localPosition = portraitPos;
        }
    }
}
