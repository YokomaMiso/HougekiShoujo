using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameStartEffect : MonoBehaviour
{

    Image baseLine;
    Text[] vsText = new Text[2];

    readonly Vector2 baseLineStartScale = new Vector3(0, 100);
    readonly Vector2 baseLineFirstTargetScale = new Vector3(1920, 100);
    readonly Vector2 baseLineEndTargetScale = new Vector3(1920, 0);

    readonly float vsTextHeight = 640;
    readonly float[] vsTextMultiple = new float[2] { 1, -1 };

    void Start()
    {
        baseLine = transform.GetChild(0).GetComponent<Image>();
        vsText[0] = transform.GetChild(1).GetComponent<Text>();
        vsText[1] = transform.GetChild(2).GetComponent<Text>();
    }

    void Update()
    {
        float timer = Managers.instance.gameManager.startTimer;

        if (timer < 0.5f) { FirstBehavior(timer / 0.5f); }
        else if (1.0f > timer && timer > 0.5f) { FirstBehavior(1); }
        else if (1.5f <= timer && timer < 2.5f) { SecondBehavior(timer - 1.5f); }

        if (timer > 3.0f) { Destroy(gameObject); }
    }

    void FirstBehavior(float _rate)
    {
        Vector2 size = Vector2.Lerp(baseLineStartScale, baseLineFirstTargetScale, _rate);
        baseLine.GetComponent<RectTransform>().sizeDelta = size;

        for (int i = 0; i < vsText.Length; i++)
        {
            float nowHeight = Mathf.Lerp(vsTextHeight * vsTextMultiple[i], 0, _rate);
            vsText[i].transform.localPosition = new Vector2(0, nowHeight);
        }
    }

    void SecondBehavior(float _rate)
    {
        Vector2 size = Vector2.Lerp(baseLineFirstTargetScale, baseLineEndTargetScale, _rate);
        baseLine.GetComponent<RectTransform>().sizeDelta = size;

        for (int i = 0; i < vsText.Length; i++)
        {
            vsText[i].color = new Color(1, 1, 1, 1.0f - _rate);
        }
    }
}
