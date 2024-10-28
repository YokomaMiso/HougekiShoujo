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

    readonly Color[] difficulityColor = new Color[3] { Color.green, Color.yellow, Color.red };
    readonly Color[] rollBGColor = new Color[3] { Color.red, Color.green, Color.blue };
    readonly string[] rollString = new string[3] { "‹ß", "’†", "‰“" };

    public void SetCharaVisual(PlayerData _pd)
    {
        for (int i = 0; i < charaIllust.Length; i++) { charaIllust[i].sprite = _pd.GetCharacterAnimData().GetCharaIllust(); }
        charaAnim.runtimeAnimatorController = _pd.GetCharacterAnimData().GetIdleAnimForUI();
        charaName.text = _pd.GetCharaName();
        charaNameRubi.text = _pd.GetCharaNameRubi();

        for (int i = 0; i < schoolIcon.Length; i++) { schoolIcon[i].sprite = _pd.GetSchoolIcon(); }

        SetDifficulity(_pd.GetDifficulity());

        SetShellType((int)_pd.GetShell().GetShellType());

        SetSkillIcon(_pd.GetShell());
        SetSkillIcon(_pd.GetSubWeapon());
    }

    void SetDifficulity(int _num)
    {
        Color applyColor = difficulityColor[_num - 1];
        for (int i = 0; i < difficultGauge.Length; i++)
        {
            if (i < _num) { difficultGauge[i].color = applyColor; }
            else { difficultGauge[i].color = Color.clear; }
        }
    }

    void SetShellType(int _num)
    {
        rollText.text = rollString[_num];
        rollBG.color = rollBGColor[_num];
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
}