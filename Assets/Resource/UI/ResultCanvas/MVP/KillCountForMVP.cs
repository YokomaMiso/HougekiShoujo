using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCountForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 12.5f;
    const float endTime = 13.5f;

    Text[] texts = new Text[2];

    readonly Color[] textColor = new Color[2] { Color.red, Color.white };

    public void SetText(ResultScoreBoard.KDFData _kdf)
    {
        for (int i = 0; i < 2; i++) { texts[i] = transform.GetChild(i).GetComponent<Text>(); }
        texts[0].text = _kdf.killCount.ToString() + "‰ñ";
    }

    void Update()
    {
        if (timer >= endTime) { return; }

        timer += Time.deltaTime;
        if (timer >= endTime) { timer = endTime; }

        float colorValue = Mathf.Clamp01((timer - startTime) / (endTime - startTime));

        for (int i = 0; i < 2; i++) { texts[i].color = textColor[i] * colorValue; }
    }
}
