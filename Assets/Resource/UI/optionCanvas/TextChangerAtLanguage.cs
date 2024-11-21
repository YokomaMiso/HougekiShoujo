using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChangerAtLanguage : MonoBehaviour
{
    public static bool change;

    Text text;
    [SerializeField] string japanese;
    [SerializeField] string english;
    [SerializeField] string simpleChinese;
    [SerializeField] string traditionalChinese;

    void ChangeText()
    {
        string applyText;
        switch (Managers.instance.nowLanguage)
        {
            default: applyText = japanese; break;
            case LANGUAGE_NUM.ENGLISH: applyText = english; break;
            case LANGUAGE_NUM.SIMPLE_CHINESE: applyText = simpleChinese; break;
            case LANGUAGE_NUM.TRADITIONAL_CHINESE: applyText = traditionalChinese; break;
        }

        text.text = applyText;
    }

    void Start()
    {
        text = GetComponent<Text>();
        ChangeText();
    }

    void Update()
    {
        if (!change) { return; }
        ChangeText();
    }

    void LateUpdate()
    {
        if (change) { change = false; }
    }
}
