using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerNameForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 12.0f;
    const float endTime = 12.5f;

    Text text;

    public void SetText(ResultScoreBoard.KDFData _kdf)
    {
        text = transform.GetComponent<Text>();

        text.text = _kdf.playerName;
    }

    void Update()
    {
        if (timer >= endTime) { return; }

        timer += Time.deltaTime;
        if (timer >= endTime) { timer = endTime; }

        float colorValue = Mathf.Clamp01((timer - startTime) / (endTime - startTime));
        text.color = Color.black * colorValue;
    }
}