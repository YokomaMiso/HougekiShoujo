using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaIllustForMVP : MonoBehaviour
{
    float timer;
    const float startTime = 9.0f;
    const float endTime = 10.0f;

    Color teamColor;
    readonly string[] teamColorCord = new string[2] { "#34b5bc", "#ff5353" };

    Image allMask;
    Image dropShadowMask;
    Image dropShadow;
    Image illust;

    public void SetSprite(ResultScoreBoard.KDFData _kdf)
    {
        allMask = transform.GetComponent<Image>();
        teamColor = ColorCordToRGB(_kdf.teamNum);

        dropShadowMask = transform.GetChild(0).GetComponent<Image>();
        dropShadow = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        illust = transform.GetChild(1).GetComponent<Image>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        illust.sprite = pd.GetCharacterAnimData().GetCharaIllust();
        dropShadowMask.sprite = illust.sprite;
    }

    void Update()
    {
        if (timer >= endTime) { return; }

        timer += Time.deltaTime;
        if (timer >= endTime) { timer = endTime; }

        float colorValue = Mathf.Clamp01((timer - startTime) / (endTime - startTime));
        allMask.color = teamColor * colorValue;
        dropShadow.color = Color.black * colorValue; 
        illust.color = Color.white * colorValue;
    }

    Color ColorCordToRGB(int _num)
    {
        if (_num >= teamColorCord.Length) { return Color.black; }

        if (ColorUtility.TryParseHtmlString(teamColorCord[_num], out Color color)) return color;
        else return Color.black;
    }
}
