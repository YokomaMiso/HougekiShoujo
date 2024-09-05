using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioBoxIndexSetting : MonoBehaviour
{
    public bool on;

    public void SetValue(bool _on)
    {
        on = _on;

        Color[] colors = new Color[2] { Color.yellow, Color.white };

        if (_on)
        {
            transform.GetChild(1).GetComponent<Image>().color = colors[0];
            transform.GetChild(2).GetComponent<Image>().color = colors[1];
        }
        else
        {
            transform.GetChild(1).GetComponent<Image>().color = colors[1];
            transform.GetChild(2).GetComponent<Image>().color = colors[0];
        }
    }
}
