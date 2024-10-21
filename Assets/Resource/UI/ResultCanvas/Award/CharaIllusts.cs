using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CharaIllusts : MonoBehaviour
{
    float timer;
    const float childSub = 1.0f;
    const float alphaStart = 2.0f;

    float limit;

    Image[] illusts = new Image[(int)AWARD_ID.MAX_NUM];

    public void SetSprite(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        transform.GetChild((int)_id).GetComponent<Image>().color = Managers.instance.ColorCordToRGB(_kdf.teamNum);

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        illusts[(int)_id] = transform.GetChild((int)_id).GetChild(0).GetComponent<Image>();
        illusts[(int)_id].sprite = pd.GetCharacterAnimData().GetCharaIllust();
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
            illusts[i].color = Color.white * colorValue;
        }
    }
}
