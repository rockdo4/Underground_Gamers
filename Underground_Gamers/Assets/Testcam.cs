using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Testcam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.DOFieldOfView(40, 1f).SetEase(Ease.OutFlash);
        //ÁÜ°ª,½Ã°£ /°î¼±ÇüÅÂ
    }

}
