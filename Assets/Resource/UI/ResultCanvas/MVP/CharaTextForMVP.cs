using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaTextForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 7.5f;
    const float endTime = 8.5f;

    Text text;

    public void SetText( ResultScoreBoard.KDFData _kdf)
    {
        text = transform.GetComponent<Text>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        text.text = pd.GetSchoolAndGrade() + "@" + pd.GetCharaName();
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
