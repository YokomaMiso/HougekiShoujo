using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChangerAtLanguageInExitCheck : TextChangerAtLanguage
{
    [SerializeField] string japanese2;
    [SerializeField] string english2;
    [SerializeField] string simpleChinese2;
    [SerializeField] string traditionalChinese2;

    public override void ChangeText()
    {
        if (text == null) { text = GetComponent<Text>(); }

        string applyText;

        if (Managers.instance.playerID == 0)
        {
            switch (Managers.instance.nowLanguage)
            {
                default: applyText = japanese; break;
                case LANGUAGE_NUM.ENGLISH: applyText = english; break;
                case LANGUAGE_NUM.SIMPLE_CHINESE: applyText = simpleChinese; break;
                case LANGUAGE_NUM.TRADITIONAL_CHINESE: applyText = traditionalChinese; break;
            }
        }
        else
        {
            switch (Managers.instance.nowLanguage)
            {
                default: applyText = japanese2; break;
                case LANGUAGE_NUM.ENGLISH: applyText = english2; break;
                case LANGUAGE_NUM.SIMPLE_CHINESE: applyText = simpleChinese2; break;
                case LANGUAGE_NUM.TRADITIONAL_CHINESE: applyText = traditionalChinese2; break;
            }
        }

        text.text = applyText;
    }
}
