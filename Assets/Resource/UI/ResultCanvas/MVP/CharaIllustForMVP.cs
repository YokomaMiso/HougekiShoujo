using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaIllustForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 11.0f;
    const float endTime = 12.0f;

    Color teamColor;

    Image mask;
    Image illust;

    public void SetSprite(ResultScoreBoard.KDFData _kdf)
    {
        mask = transform.GetComponent<Image>();
        teamColor = Managers.instance.ColorCordToRGB(_kdf.teamNum);

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        illust = transform.GetChild(0).GetComponent<Image>();
        illust.sprite = pd.GetCharacterAnimData().GetCharaIllust();
    }

    void Update()
    {
        if (timer >= endTime) { return; }

        timer += Time.deltaTime;
        if (timer >= endTime) { timer = endTime; }

        float colorValue = Mathf.Clamp01((timer - startTime) / (endTime - startTime));
        mask.color = teamColor * colorValue;
        illust.color = Color.white * colorValue;
    }
}
