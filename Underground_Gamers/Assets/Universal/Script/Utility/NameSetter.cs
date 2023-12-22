using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NameSetter : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public int StartNum;

    //public void Awake()
    //{
    //    // 기존 파일 경로 및 파일명
    //    foreach (GameObject go in gameObjects)
    //    {
    //        string oldPath = AssetDatabase.GetAssetPath(go);
    //        string oldFileName = Path.GetFileNameWithoutExtension(oldPath);

    //        // 새로운 파일명
    //        string newFileName = $"{StartNum++}"; // 원하는 새로운 파일명으로 변경

    //        // 프리팹 복사
    //        string newPath = oldPath.Replace(oldFileName, newFileName);
    //        AssetDatabase.CopyAsset(oldPath, newPath);

    //        // 기존 프리팹 삭제
    //        AssetDatabase.DeleteAsset(oldPath);
    //    }

    //}
}
