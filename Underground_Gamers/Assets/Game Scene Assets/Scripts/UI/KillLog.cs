using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLog : MonoBehaviour
{
    public KillLogPanel killLogPanel;



    public Transform killerPortraitParent;
    public Transform deadPortraitParent;
    public float scale;
    public float destroyedTimer;
    private float duration = 2f;

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

    public void SetKillLog(GameObject killerPortrait, GameObject deadPortrait)
    {
        GameObject k_portrait = Instantiate(killerPortrait, killerPortraitParent);
        GameObject d_portrait = Instantiate(deadPortrait, deadPortraitParent);
        SetPortrait(k_portrait, killerPortraitParent);
        SetPortrait(d_portrait, deadPortraitParent);
    }

    private void SetPortrait(GameObject portrait, Transform parent)
    {
        portrait.layer = LayerMask.NameToLayer("OverUI");
        portrait.transform.localScale = Vector3.one * scale;
        portrait.transform.position = parent.position;
    }
}
