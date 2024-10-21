using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaTexts : MonoBehaviour
{
    float timer;
    const float childSub = 1.0f;
    const float alphaStart = 1.0f;

    float limit;

    Text[] texts = new Text[(int)AWARD_ID.MAX_NUM];

    public void SetText(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        texts[(int)_id] = transform.GetChild((int)_id).GetComponent<Text>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        texts[(int)_id].text = pd.GetSchoolAndGrade() + "\n" + pd.GetCharaName();
    }
    void Start()
    {
        limit = alphaStart + childSub * (int)AWARD_ID.MAX_NUM;
    }
    void Update()
    {
        if (timer >= limit) { return; }

        timer += Time.deltaTime;
        if (timer >= limit) { timer = limit; }

        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++)
        {
            float colorValue = Mathf.Clamp01(timer - alphaStart - (childSub * i));
            texts[i].color = Color.white * colorValue;
        }
    }
}
