using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Create/SpriteData/CharacterAnim", order = 1)]
public class CharacterAnimData : ScriptableObject
{
    [SerializeField, Header("Character Icon")] protected Sprite characterIcon;
    [SerializeField, Header("Character Idle")] protected Sprite idleFirstSprite;
    [SerializeField, Header("Character Illust")] protected Sprite charaIllust;

    [SerializeField, Header("Character Idle")] protected RuntimeAnimatorController idleAnimationData;
    [SerializeField, Header("Character Run")] protected RuntimeAnimatorController[] runAnimationData;
    [SerializeField, Header("Character Reload")] protected RuntimeAnimatorController reloadAnimationData;
    [SerializeField, Header("Character Aim")] protected RuntimeAnimatorController[] aimAnimationData;
    [SerializeField, Header("Character Recoil")] protected RuntimeAnimatorController[] recoilAnimationData;
    [SerializeField, Header("Character Dead")] protected RuntimeAnimatorController deadAnimationData;

    public Sprite GetCharaIcon() { return characterIcon; }
    public Sprite GetCharaIdle() { return idleFirstSprite; }
    public Sprite GetCharaIllust() { return charaIllust; }
    public virtual RuntimeAnimatorController GetIdleAnim(CANON_STATE _state = CANON_STATE.EMPTY) { return idleAnimationData; }
    public RuntimeAnimatorController[] GetRunAnims() { return runAnimationData; }
    public virtual RuntimeAnimatorController GetRunAnim(int _num,CANON_STATE _state = CANON_STATE.EMPTY) { return runAnimationData[_num]; }
    public RuntimeAnimatorController GetReloadAnim() { return reloadAnimationData; }
    public RuntimeAnimatorController[] GetAimAnims() { return aimAnimationData; }
    public RuntimeAnimatorController GetAimAnim(int _num) { return aimAnimationData[_num]; }
    public RuntimeAnimatorController[] GetRecoilAnims() { return recoilAnimationData; }
    public RuntimeAnimatorController GetRecoilAnim(int _num) { return recoilAnimationData[_num]; }
    public RuntimeAnimatorController GetDeadAnim() { return deadAnimationData; }
}