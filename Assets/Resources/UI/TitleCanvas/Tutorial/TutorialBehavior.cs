using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBehavior : MonoBehaviour
{
    GameObject textInstance;
    [SerializeField] GameObject[] texts;
    LANGUAGE_NUM languageNum;

    void TextSpawner()
    {
        languageNum = Managers.instance.nowLanguage;
        if (textInstance != null) { Destroy(textInstance); }
        textInstance = Instantiate(texts[(int)languageNum], transform);
    }

    void Start()
    {
        TextSpawner();
    }

    void OnEnable()
    {
        ChangeDisplayButtons();
        TextSpawner();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }
    }

    protected virtual void ChangeDisplayButtons()
    {

    }
}
