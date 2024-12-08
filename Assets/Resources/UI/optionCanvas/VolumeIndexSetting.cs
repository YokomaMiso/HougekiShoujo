using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeIndexSetting : MonoBehaviour
{
    [SerializeField] float minValue = 0;
    [SerializeField] float maxValue = 1;
    float nowValue;

    Slider slider;

    void Init()
    {
        slider = transform.GetChild(1).GetComponent<Slider>();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    void Update()
    {
        if (slider.value != nowValue)
        {
            nowValue = slider.value;
            ValueChange();
        }
    }

    public void SetValue(float _value)
    {
        Init();
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

        slider.value = nowValue;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().text = nowValue.ToString("f1");
        Managers.instance.PlaySFXForUI(2);
    }
}
