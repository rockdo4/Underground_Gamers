using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public CutScene cutscenePrefab;
    public Transform parent;
    public Vector3 targetPos;
    public float moveDuration = 1f;

    public float fadeDuration = 0.5f;

    public void CreateCutScene()
    {
        CutScene cutscene = Instantiate(cutscenePrefab, parent);
        cutscene.cutSceneImage.GetComponent<RectTransform>().DOAnchorPos(targetPos, moveDuration).SetEase(Ease.OutQuint).OnComplete(() => FadeCutScene(cutscene));
    }

    public void FadeCutScene(CutScene cutScene)
    {
        cutScene.cutSceneImage.DOFade(0f, fadeDuration).OnComplete(()=> DestroyCutScene(cutScene));
    }

    public void DestroyCutScene(CutScene cutScene)
    {
        Destroy(cutScene.gameObject);
        //cutScene.gameObject.SetActive(false);
    }
}
