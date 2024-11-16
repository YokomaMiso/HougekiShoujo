using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockWindow : MonoBehaviour
{
    float timer;
    const float lifeTime = 3.0f;

    Text unlockText;
    readonly string[] textString = new string[3]
    {
        "「ハードコアモード」を起動",
        "「近衛翼」を解放しました",
        "「ぺっぺろ丸」を解放しました",
    };

    int textNum;
    public void SetTextNum(int _num) { textNum = _num; }

    void Start()
    {
        unlockText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        unlockText.text = textString[textNum];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
