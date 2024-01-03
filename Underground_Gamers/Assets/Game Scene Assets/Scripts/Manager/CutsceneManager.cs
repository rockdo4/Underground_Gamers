using DG.Tweening;
using System;
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

    private Queue<CutScene> cutsceneQueue = new Queue<CutScene>();
    private int maxConcurrentCutscenes = 2;
    private int runningCutscenes = 0;

    public void CreateCutScene()
    {
        if (runningCutscenes < maxConcurrentCutscenes)
        {
            CutScene cutscene = Instantiate(cutscenePrefab, parent);
            cutsceneQueue.Enqueue(cutscene);
            runningCutscenes++;
            PlayNextCutScene();
        }
    }

    private void PlayNextCutScene()
    {
        if (cutsceneQueue.Count > 0)
        {
            CutScene nextCutScene = cutsceneQueue.Dequeue();

            nextCutScene.cutSceneImage.GetComponent<RectTransform>().DOAnchorPos(targetPos, moveDuration)
                .SetEase(Ease.OutQuint).OnComplete(() => FadeCutScene(nextCutScene));
        }
    }

    public void FadeCutScene(CutScene cutScene)
    {
        cutScene.cutSceneImage.DOFade(0f, fadeDuration).OnComplete(() => DestroyCutScene(cutScene));
    }

    public void DestroyCutScene(CutScene cutScene)
    {
        Destroy(cutScene.gameObject);
        runningCutscenes--; 
        PlayNextCutScene();
    }
}
