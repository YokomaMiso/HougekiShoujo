using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardCount : MonoBehaviour
{
    enum TEXT_ID { COUNT = 0, UNIT, MAX_NUM };

    float timer;
    const float childSub = 0.25f;
    const float alphaStart = 2.5f;

    float limit;

    Text[][] texts = new Text[(int)AWARD_ID.MAX_NUM][]
    {
        new Text[(int)TEXT_ID.MAX_NUM],
        new Text[(int)TEXT_ID.MAX_NUM],
        new Text[(int)TEXT_ID.MAX_NUM]
    };

    readonly string[] units = new string[(int)LANGUAGE_NUM.MAX_NUM] { "回", "x", "次", "次" };
    readonly string[][] actionText = new string[(int)AWARD_ID.MAX_NUM][]
    {
        new string[(int)LANGUAGE_NUM.MAX_NUM]{"撃った","Fired", "射击", "射擊" },
        new string[(int)LANGUAGE_NUM.MAX_NUM]{"倒された", "KO-ed", "被击倒", "被擊倒" },
        new string[(int)LANGUAGE_NUM.MAX_NUM]{"誤射した","Misfired", "误伤", "誤傷" }
    };
    readonly Color[] textColor = new Color[2] { Color.red, Color.white };

    public void SetText(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        for (int i = 0; i < 2; i++)
        {
            texts[(int)_id][i] = transform.GetChild((int)_id).GetChild(i).GetComponent<Text>();
        }

        string unit = units[(int)Managers.instance.nowLanguage];
        switch (_id)
        {
            case AWARD_ID.JUNKY:
                texts[(int)_id][(int)TEXT_ID.COUNT].text = _kdf.fireCount.ToString() + unit;
                break;

            case AWARD_ID.VICTIM:
                texts[(int)_id][(int)TEXT_ID.COUNT].text = _kdf.deathCount.ToString() + unit;
                break;

            case AWARD_ID.DANGER:
                texts[(int)_id][(int)TEXT_ID.COUNT].text = _kdf.friendlyFireCount.ToString() + unit;
                break;
        }

        texts[(int)_id][(int)TEXT_ID.UNIT].text = actionText[(int)_id][(int)Managers.instance.nowLanguage];
    }
    void Start()
    {
        limit = alphaStart + childSub * ((int)AWARD_ID.MAX_NUM + 1);
    }
    void Update()
    {
        if (timer >= limit) { return; }

        timer += Time.deltaTime;
        if (timer >= limit) { timer = limit; }

        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++)
        {
            float subValue = alphaStart + childSub * (i - 1);
            float colorValue = Mathf.Clamp01((timer - subValue) / (alphaStart + childSub * i - subValue));

            for (int j = 0; j < (int)TEXT_ID.MAX_NUM; j++)
            {
                texts[i][j].color = textColor[j] * colorValue;
            }
        }
    }
}
