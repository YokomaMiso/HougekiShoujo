using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Create/SpriteData/CharacterAnim", order = 1)]
public class CharacterAnimData : ScriptableObject
{
    [SerializeField, Header("Character Icon")] protected Sprite characterIcon;
    [SerializeField, Header("Character Idle")] protected Sprite idleFirstSprite;
    [SerializeField, Header("Character Illust")] protected Sprite charaIllust;
    [SerializeField, Header("Character Idle For UI")] protected RuntimeAnimatorController idleForUIAnimationData;
    [SerializeField, Header("Character Run For UI")] protected RuntimeAnimatorController runForUIAnimationData;

    [SerializeField, Header("Character Idle")] protected RuntimeAnimatorController idleAnimationData;
    [SerializeField, Header("Character Run")] protected RuntimeAnimatorController[] runAnimationData;
    [SerializeField, Header("Character Reload")] protected RuntimeAnimatorController reloadAnimationData;
    [SerializeField, Header("Character Aim")] protected RuntimeAnimatorController[] aimAnimationData;
    [SerializeField, Header("Character Recoil")] protected RuntimeAnimatorController[] recoilAnimationData;
    [SerializeField, Header("Character Dead")] protected RuntimeAnimatorController deadAnimationData;

    public Sprite GetCharaIcon() { return characterIcon; }
    public Sprite GetCharaIdle() { return idleFirstSprite; }
    public Sprite GetCharaIllust() { return charaIllust; }
    public virtual RuntimeAnimatorController GetIdleAnimForUI() { return idleForUIAnimationData; }
    public virtual RuntimeAnimatorController GetRunAnimForUI() { return runForUIAnimationData; }
    public virtual RuntimeAnimatorController GetIdleAnim(CANON_STATE _state = CANON_STATE.EMPTY, float _direction = 1) { return idleAnimationData; }
    public RuntimeAnimatorController[] GetRunAnims() { return runAnimationData; }
    public virtual RuntimeAnimatorController GetRunAnim(int _num, CANON_STATE _state = CANON_STATE.EMPTY, float _direction = 1) { return runAnimationData[_num]; }
    public virtual RuntimeAnimatorController GetReloadAnim(float _direction = 1) { return reloadAnimationData; }
    public  RuntimeAnimatorController[] GetAimAnims() { return aimAnimationData; }
    public virtual RuntimeAnimatorController GetAimAnim(int _num, float _direction = 1) { return aimAnimationData[_num]; }
    public  RuntimeAnimatorController[] GetRecoilAnims() { return recoilAnimationData; }
    public virtual RuntimeAnimatorController GetRecoilAnim(int _num, float _direction = 1) { return recoilAnimationData[_num]; }
    public RuntimeAnimatorController GetDeadAnim() { return deadAnimationData; }
}