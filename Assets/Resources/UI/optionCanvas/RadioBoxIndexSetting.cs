using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioBoxIndexSetting : MonoBehaviour
{
    public bool on;
    public static Sprite[] levelFourWindow = new Sprite[2];
    static Color[] colors = new Color[2] { Color.white, Color.black };

    public void SetValue(bool _on)
    {
        on = _on;

        if (_on)
        {
            transform.GetChild(1).GetComponent<Image>().sprite = levelFourWindow[1];
            transform.GetChild(1).GetChild(0).GetComponent<Text>().color = colors[1];
            transform.GetChild(2).GetComponent<Image>().sprite = levelFourWindow[0];
            transform.GetChild(2).GetChild(0).GetComponent<Text>().color = colors[0];
        }
        else
        {
            transform.GetChild(1).GetComponent<Image>().sprite = levelFourWindow[0];
            transform.GetChild(1).GetChild(0).GetComponent<Text>().color = colors[0];
            transform.GetChild(2).GetComponent<Image>().sprite = levelFourWindow[1];
            transform.GetChild(2).GetChild(0).GetComponent<Text>().color = colors[1];
        }

        Managers.instance.PlaySFXForUI(2);
    }
}
