using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaVisualInRoomCanvas : MonoBehaviour
{
    [SerializeField] Image[] charaIllust;
    [SerializeField] Animator charaAnim;
    [SerializeField] Image[] schoolIcon;
    [SerializeField] Text charaName;
    [SerializeField] Text charaNameRubi;
    [SerializeField] Image[] difficultGauge;
    [SerializeField] Text rollText;
    [SerializeField] Image rollBG;

    [SerializeField] Image[] skillIcon;
    [SerializeField] Text[] weaponName;
    [SerializeField] Text[] weaponExplain;

    readonly string[] difficulityColor = new string[3] { "#5dff61", "#ffef5d", "#ff5353" };
    readonly string[] rollBGColor = new string[3] { "#5dff61", "#ffef5d", "#ff5353" };
    readonly string[] rollString = new string[3] { "‹ß", "’†", "‰“" };

    readonly Vector3 illustStartPos = new Vector3(1150, 32);
    readonly Vector3 illustEndPos = new Vector3(0, 32);
    readonly Vector3 illustDropShadowPos = new Vector3(-52, 48);

    float timer;
    const float arriveTime = 0.25f;
    const float dropShadowArriveTime = 0.15f;
    const float endTime = arriveTime + dropShadowArriveTime;

    PlayerData nowChara;

    void Update()
    {
        if (timer >= endTime) { return; }
        timer += Time.deltaTime;
        if (timer >= endTime) { timer = endTime; }

        if (timer <= arriveTime)
        {
            for (int i = 0; i < charaIllust.Length; i++)
            {
                float nowRate = timer / arriveTime;
                nowRate = Mathf.Sqrt(nowRate);
                charaIllust[i].transform.localPosition = Vector3.Lerp(illustStartPos, illustEndPos, nowRate);
            }
        }
        else if (timer <= endTime)
        {
            float nowRate = (timer - arriveTime) / (endTime - arriveTime);
            nowRate = Mathf.Sqrt(nowRate);
            charaIllust[0].transform.localPosition = Vector3.Lerp(illustEndPos, illustDropShadowPos, nowRate);
            charaIllust[1].transform.localPosition = illustEndPos;
        }
    }

    public void SetCharaVisual(PlayerData _pd)
    {
        if (_pd == null || _pd == nowChara) { return; }
        nowChara = _pd;

        for (int i = 0; i < charaIllust.Length; i++) { charaIllust[i].sprite = _pd.GetCharacterAnimData().GetCharaIllust(); }
        charaAnim.runtimeAnimatorController = _pd.GetCharacterAnimData().GetIdleAnimForUI();
        charaName.text = _pd.GetCharaName();
        charaNameRubi.text = _pd.GetCharaNameRubi();

        for (int i = 0; i < schoolIcon.Length; i++) { schoolIcon[i].sprite = _pd.GetSchoolIcon(); }

        SetDifficulity(_pd.GetDifficulity());

        SetShellType((int)_pd.GetShell().GetShellType());

        SetSkillIcon(_pd.GetShell());
        SetSkillIcon(_pd.GetSubWeapon());

        timer = 0;
        for (int i = 0; i < charaIllust.Length; i++) { charaIllust[i].transform.localPosition = illustStartPos; }
    }

    void SetDifficulity(int _num)
    {
        Color applyColor = ColorCordToRGB(difficulityColor[_num - 1]);
        for (int i = 0; i < difficultGauge.Length; i++)
        {
            if (i < _num) { difficultGauge[i].color = applyColor; }
            else { difficultGauge[i].color = Color.clear; }
        }
    }

    void SetShellType(int _num)
    {
        rollText.text = rollString[_num];
        rollBG.color = ColorCordToRGB(rollBGColor[_num]);
    }

    void SetSkillIcon(Shell _shell)
    {
        skillIcon[0].sprite = _shell.GetShellIcon();
        weaponName[0].text = _shell.GetShellName();
        weaponExplain[0].text = _shell.GetShellExplain();
    }
    void SetSkillIcon(SubWeapon _sub)
    {
        skillIcon[1].sprite = _sub.GetIcon();
        weaponName[1].text = _sub.GetSubWeaponName();
        weaponExplain[1].text = _sub.GetSubWeaponExplain();
    }

    Color ColorCordToRGB(string _cord)
    {
        if (ColorUtility.TryParseHtmlString(_cord, out Color color)) return color;
        else return Color.black;
    }
}