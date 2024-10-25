using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButtonBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    Image image;

    int state = 0;
    float timer;
    readonly float[] maxTime = new float[2] { 0.2f, 0.115f };

    float sizeRate;

    public void SetState(int _num) { state = _num; }

    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                TimerUpdate(-Time.deltaTime);
                Shrink();
                break;

            case 1:
                TimerUpdate(Time.deltaTime);
                Expand();
                break;

        }

        image.sprite = sprites[state];
    }

    void TimerUpdate(float _addTime)
    {
        timer = Mathf.Clamp(timer + _addTime, 0, maxTime[0]);
        sizeRate = Mathf.Clamp01(timer / maxTime[state]);
    }

    void Expand()
    {
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, Mathf.Sqrt(sizeRate));
    }
    void Shrink()
    {
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, Mathf.Pow(sizeRate, 2));
    }
}
