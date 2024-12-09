using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameStartEffect : MonoBehaviour
{
    Image baseLine;

    readonly Vector2 baseLineStartScale = new Vector3(1920, 688);
    readonly Vector2 baseLineEndTargetScale = new Vector3(1920, 0);

    readonly Vector2 startPos = new Vector2(0, -32);
    readonly Vector2 endPos = new Vector2(-1920*2, -32);

    void Start() { baseLine = transform.GetChild(0).GetComponent<Image>(); }

    void Update()
    {
        IngameData.GameData hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData;
        float timer = hostIngameData.startTimer;

        if (1.5f <= timer && timer < 3.0f)
        {
            float timeRate = Mathf.Pow(Mathf.Clamp01(timer - 1.5f),3);
            LineBehavior(timeRate);
        }

        if (timer >= 3.0f) { Destroy(gameObject); }
    }

    void LineBehavior(float _rate)
    {
        //Vector2 size = Vector2.Lerp(baseLineStartScale, baseLineEndTargetScale, _rate);
        //baseLine.GetComponent<RectTransform>().sizeDelta = size;

        Vector2 pos = Vector2.Lerp(startPos, endPos, _rate);
        baseLine.transform.localPosition = pos;
    }
}
