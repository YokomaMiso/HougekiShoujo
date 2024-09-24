using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Create/SpriteData/CharacterAnim", order = 1)]
public class CharacterAnimData : ScriptableObject
{
    [SerializeField, Header("Character Icon")] Sprite characterIcon;
    [SerializeField, Header("Character Idle")] Sprite idleFirstSprite;

    [SerializeField, Header("Character Idle")] RuntimeAnimatorController idleAnimationData;
    [SerializeField, Header("Character Run")] RuntimeAnimatorController[] runAnimationData;
    [SerializeField, Header("Character Reload")] RuntimeAnimatorController reloadAnimationData;
    [SerializeField, Header("Character Aim")] RuntimeAnimatorController[] aimAnimationData;
    [SerializeField, Header("Character Recoil")] RuntimeAnimatorController[] recoilAnimationData;
    [SerializeField, Header("Character Dead")] RuntimeAnimatorController deadAnimationData;

    public Sprite GetCharaIcon() { return characterIcon; }
    public Sprite GetCharaIdle() { return idleFirstSprite; }
    public RuntimeAnimatorController GetIdleAnim() { return idleAnimationData; }
    public RuntimeAnimatorController[] GetRunAnims() { return runAnimationData; }
    public RuntimeAnimatorController GetRunAnim(int _num) { return runAnimationData[_num]; }
    public RuntimeAnimatorController GetReloadAnim() { return reloadAnimationData; }
    public RuntimeAnimatorController[] GetAimAnims() { return aimAnimationData; }
    public RuntimeAnimatorController GetAimAnim(int _num) { return aimAnimationData[_num]; }
    public RuntimeAnimatorController[] GetRecoilAnims() { return recoilAnimationData; }
    public RuntimeAnimatorController GetRecoilAnim(int _num) { return recoilAnimationData[_num]; }
    public RuntimeAnimatorController GetDeadAnim() { return deadAnimationData; }
}