using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class CharaIllusts : MonoBehaviour
{
    float timer;
    const float childSub = 0.5f;
    const float alphaStart = 1.0f;

    float limit;
    Color[] teamColors = new Color[(int)AWARD_ID.MAX_NUM];
    readonly string[] teamColorCord = new string[2] { "#34b5bc", "#ff5353" };

    Image[] allMasks = new Image[(int)AWARD_ID.MAX_NUM];
    Image[] illusts = new Image[(int)AWARD_ID.MAX_NUM];

    bool[] sfxPlayed = new bool[(int)AWARD_ID.MAX_NUM] { false, false, false };
    [SerializeField] AudioClip[] popSFX;

    public void SetSprite(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        allMasks[(int)_id] = transform.GetChild((int)_id).GetComponent<Image>();
        teamColors[(int)_id] = ColorCordToRGB(_kdf.teamNum);

        illusts[(int)_id] = transform.GetChild((int)_id).GetChild(0).GetComponent<Image>();

        PlayerData pd = Managers.instance.gameManager.playerDatas[_kdf.characterID];
        illusts[(int)_id].sprite = pd.GetCharacterAnimData().GetCharaIllust();
    }
    void Start()
    {
        limit = alphaStart + (childSub * (int)AWARD_ID.MAX_NUM + 1);
    }
    void Update()
    {
        if (timer >= limit) { return; }

        timer += Time.deltaTime;
        if (timer >= limit) { timer = limit; }

        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++)
        {
            float colorValue = Mathf.Clamp01(timer - alphaStart - (childSub * i));
            allMasks[i].color = teamColors[i] * colorValue;
            illusts[i].color = Color.white * colorValue;

            if (!sfxPlayed[i]) 
            {
                if (colorValue > 0)
                {
                    SoundManager.PlaySFXForUI(popSFX[i]);
                    sfxPlayed[i] = true;
                }
            }
        }
    }

    Color ColorCordToRGB(int _num)
    {
        if (_num >= teamColorCord.Length) { return Color.black; }

        if (ColorUtility.TryParseHtmlString(teamColorCord[_num], out Color color)) return color;
        else return Color.black;
    }
}
