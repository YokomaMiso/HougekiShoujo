using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoader : MonoBehaviour
{
    readonly string path = "Assets / Resources / UI / InGameCanvas / IngameUIs.prefab";

    private void Start()
    {
        //Resources/Background1��񓯊��Ń��[�h����
        StartCoroutine(LoadAsync(path));
    }

    //�񓯊��Ń��[�h����
    private IEnumerator LoadAsync(string filePath)
    {
        //�񓯊����[�h�J�n
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(filePath);

        //���[�h���I���܂őҋ@(resourceRequest.progress�Ői�������m�F�o����)
        while (!resourceRequest.isDone) { yield return 0; }

        //���[�h����
        Destroy(this);
    }
}
