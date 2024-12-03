using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoader : MonoBehaviour
{
    readonly string path = "Assets / Resources / UI / InGameCanvas / IngameUIs.prefab";

    private void Start()
    {
        //Resources/Background1を非同期でロードする
        StartCoroutine(LoadAsync(path));
    }

    //非同期でロードする
    private IEnumerator LoadAsync(string filePath)
    {
        //非同期ロード開始
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(filePath);

        //ロードが終わるまで待機(resourceRequest.progressで進捗率を確認出来る)
        while (!resourceRequest.isDone) { yield return 0; }

        //ロード完了
        Destroy(this);
    }
}
