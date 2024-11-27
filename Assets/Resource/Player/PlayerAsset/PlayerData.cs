using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Character Name")]
    [SerializeField, Header("Character Name")] string charaName;
    [SerializeField, Header("Character Name Rubi")] string charaNameRubi;
    [SerializeField, Header("Character Name Rubi ENG")] string charaNameRubiEnglish;
    [SerializeField, Header("School")] SchoolData schoolData;
    [SerializeField, Header("Grade")] int grade;
    [SerializeField, Header("MVP Explain"), TextArea(1, 5)] string[] mvpExplain;
    [Header("Animation")]
    [SerializeField, Header("Anim Data")] CharacterAnimData characterAnimData;
    [Header("Status Data")]
    [SerializeField, Header("Move Speed")] float moveSpeed = 5;
    [SerializeField, Header("Difficulity")] int difficulity = 1;
    [Header("Shell Data")]
    [SerializeField, Header("Shell")] Shell shell;
    [Header("Weapon Data")]
    [SerializeField, Header("Sub Weapon")] SubWeapon subWeapon;

    [Header("SFX")]
    [SerializeField, Header("Player SFX Data")] PlayerSFXData playerSFXData;
    [Header("Voice")]
    [SerializeField, Header("Player Voice Data")] PlayerVoiceData playerVoiceData;
    [Header("Result Jingle")]
    [SerializeField, Header("Player Jingle")] AudioClip resultJingle;

    public string GetCharaName() { return charaName; }
    public string GetCharaNameRubi() 
    {
        if (Managers.instance.nowLanguage == LANGUAGE_NUM.JAPANESE) { return charaNameRubi; }
        else { return charaNameRubiEnglish; }
    }
    public string GetSchoolName() { return schoolData.GetSchoolName(); }
    public Sprite GetSchoolIcon() { return schoolData.GetSchoolIcon(); }
    public string GetGrade() 
    {
        if (grade == 0) { return " 教員"; }
        return grade.ToString() + " 年"; 
    }
    public CharacterAnimData GetCharacterAnimData() { return characterAnimData; }
    public string GetMVPExplain() { return mvpExplain[(int)Managers.instance.nowLanguage]; }

    public float GetMoveSpeed() { return moveSpeed; }
    public int GetDifficulity() { return difficulity; }

    public Shell GetShell() { return shell; }
    public SubWeapon GetSubWeapon() { return subWeapon; }

    public PlayerSFXData GetPlayerSFXData() { return playerSFXData; }
    public PlayerVoiceData GetPlayerVoiceData() { return playerVoiceData; }
    public AudioClip GetResultJingle() { return resultJingle; }
}
