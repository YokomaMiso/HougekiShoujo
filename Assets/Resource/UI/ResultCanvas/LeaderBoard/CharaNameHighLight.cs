using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaNameHighLight : MonoBehaviour
{
    float timer;
    const float openTime = 0.5f;
    readonly Vector2[] rectSize = new Vector2[2] { new Vector2(0, 1500), new Vector2(300, 1500) };

    string familyName;
    string firstName;

    RectTransform rectTransform;

    bool open = true;
    public void Close() { open = false; }

    public void SetName(PlayerData _pd)
    {
        rectTransform = GetComponent<RectTransform>();

        string charaName = _pd.GetCharaName();
        int endIndex = 0;
        for (int i = 0; i < charaName.Length; i++)
        {
            if (charaName[i] == ' ') { endIndex = i; break; }
            familyName += charaName[i];
        }

        for (int i = endIndex + 1; i < charaName.Length; i++)
        {
            firstName += charaName[i];
        }

        transform.GetChild(1).GetComponent<Text>().text = familyName;
        transform.GetChild(2).GetComponent<Text>().text = firstName;
        transform.GetChild(3).GetComponent<Text>().text = firstName;
    }

    void Update()
    {
        if (open) { TimerAdd(); }
        else { TimerSub(); }

        float nowRate;
        nowRate = Mathf.Clamp01(timer / openTime);
        rectTransform.sizeDelta = Vector2.Lerp(rectSize[0], rectSize[1], nowRate);
    }

    void TimerAdd()
    {
        if (timer >= openTime) { return; }

        timer += Time.deltaTime;
        if (timer >= openTime) { timer = openTime; }
    }

    void TimerSub()
    {
        if (timer <= 0) { return; }

        timer -= Time.deltaTime * 4;
        if (timer <= 0) { timer = 0; }
    }
}
