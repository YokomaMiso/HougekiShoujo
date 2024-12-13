using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCountForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 10.0f;
    const float endTime = 10.5f;

    Text[] texts = new Text[2];

    readonly Color[] textColor = new Color[2] { Color.red, Color.white };

    readonly string[] units = new string[(int)LANGUAGE_NUM.MAX_NUM] { "人", "x", "人", "人" };
    readonly string[] actionText = new string[(int)LANGUAGE_NUM.MAX_NUM] { "倒した", "killed", "击败", "擊敗" };

    public void SetText(ResultScoreBoard.KDFData _kdf)
    {
        for (int i = 0; i < 2; i++) { texts[i] = transform.GetChild(i).GetComponent<Text>(); }
        texts[0].text = _kdf.killCount.ToString() + units[(int)Managers.instance.nowLanguage];
        texts[1].text = actionText[(int)Managers.instance.nowLanguage];
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
