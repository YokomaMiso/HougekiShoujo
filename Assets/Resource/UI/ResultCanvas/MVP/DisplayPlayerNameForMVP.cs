using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerNameForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 9.3f;
    const float endTime = 9.8f;

    Text text;

    bool jinglePlayed;
    AudioClip mvpJingle;

    public void SetText(ResultScoreBoard.KDFData _kdf)
    {
        text = transform.GetComponent<Text>();

        text.text = _kdf.playerName;
        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        mvpJingle = pd.GetResultJingle();

    }

    void Update()
    {
        if (timer >= endTime) { return; }

        timer += Time.deltaTime;
        if (timer >= endTime) { timer = endTime; }

        float colorValue = Mathf.Clamp01((timer - startTime) / (endTime - startTime));
        text.color = Color.black * colorValue;

        if (jinglePlayed) { return; }
        if (timer >= startTime)
        {
            SoundManager.PlayJingleForResult(mvpJingle);
            jinglePlayed = true;
        }
    }
}
