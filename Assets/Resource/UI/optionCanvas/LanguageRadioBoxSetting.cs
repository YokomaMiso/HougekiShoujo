using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageRadioBoxSetting : MonoBehaviour
{
    public int num;
    const int maxNum = 4;

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
        Color[] colors = new Color[2] { Color.yellow, Color.white };

        for (int i = 0; i < maxNum; i++)
        {
            Color color = colors[1];
            if (num == i) { color = colors[0]; }
            transform.GetChild(i + 1).GetComponent<Image>().color = color;
        }
    }
}
