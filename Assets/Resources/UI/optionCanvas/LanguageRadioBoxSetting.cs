using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageRadioBoxSetting : MonoBehaviour
{
    public int num;
    const int maxNum = 4;

    public static Sprite[] levelFourWindow = new Sprite[2];

    public void SetValue(int _num)
    {
        num = _num;
        ColorUpdate();

        Managers.instance.nowLanguage = (LANGUAGE_NUM)num;
        TextChangerAtLanguage.change = true;
    }

    public int AddValue(int _num)
    {
        if (_num > 0) { num = (num + 1) % maxNum; }
        else { num = (num + maxNum - 1) % maxNum; }

        ColorUpdate();

        Managers.instance.nowLanguage = (LANGUAGE_NUM)num;
        TextChangerAtLanguage.change = true;

        return num;
    }

    void ColorUpdate()
    {
        for (int i = 0; i < maxNum; i++)
        {
            Sprite sprite = levelFourWindow[0];
            Color color = Color.white;
            if (num == i)
            {
                sprite = levelFourWindow[1];
                color = Color.black;
            }
            transform.GetChild(i + 1).GetComponent<Image>().sprite = sprite;
            transform.GetChild(i + 1).GetChild(0).GetComponent<Text>().color = color;
        }
    }
}
