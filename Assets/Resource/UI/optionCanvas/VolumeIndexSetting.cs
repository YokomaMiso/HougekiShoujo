using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeIndexSetting : MonoBehaviour
{
    [SerializeField] float minValue = 0;
    [SerializeField] float maxValue = 1;
    float nowValue;

    void Start()
    {
        transform.GetChild(1).GetComponent<Slider>().minValue = minValue;
        transform.GetChild(1).GetComponent<Slider>().maxValue = maxValue;
    }

    public void SetValue(float _value)
    {
        nowValue = _value;
        ValueChange();
    }
    public void AddValue(float _value)
    {
        nowValue += _value * (maxValue / 10);
        ValueChange();
    }
    public float GetValue() { return nowValue; }

    void ValueChange()
    {
        if (nowValue < minValue) { nowValue = minValue; }
        if (nowValue > maxValue) { nowValue = maxValue; }

        transform.GetChild(1).GetComponent<Slider>().value = nowValue;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().text = nowValue.ToString("f1");
    }
}
