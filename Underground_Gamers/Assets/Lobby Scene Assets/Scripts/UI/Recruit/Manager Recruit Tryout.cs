using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRecruitTryout : ManagerRecruit
{
    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

}
