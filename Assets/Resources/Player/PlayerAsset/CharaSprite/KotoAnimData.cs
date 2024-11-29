using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Create/SpriteData/CharacterAnim/KotoData", order = 1)]
public class KotoAnimData : CharacterAnimData
{
    [SerializeField, Header("Character Idle2")] RuntimeAnimatorController idleAnimationData2;
    [SerializeField, Header("Character Run2")] RuntimeAnimatorController[] runAnimationData2;
    [SerializeField, Header("Character Reload2")] RuntimeAnimatorController reloadAnimationData2;
    [SerializeField, Header("Character Aim2")] RuntimeAnimatorController[] aimAnimationData2;
    [SerializeField, Header("Character Recoil2")] RuntimeAnimatorController[] recoilAnimationData2;

    public override RuntimeAnimatorController GetIdleAnim(CANON_STATE _state = CANON_STATE.EMPTY, float _direction = 1)
    {
        if (_direction > 0) { return idleAnimationData; }
        else { return idleAnimationData2; }
    }
    public override RuntimeAnimatorController GetRunAnim(int _num, CANON_STATE _state = CANON_STATE.EMPTY, float _direction = 1)
    {
        if (_direction > 0) { return runAnimationData[_num]; }
        else { return runAnimationData2[_num]; }
    }
    public override RuntimeAnimatorController GetReloadAnim(float _direction = 1)
    {
        if (_direction > 0) { return reloadAnimationData; }
        else { return reloadAnimationData2; }
    }
    public override RuntimeAnimatorController GetAimAnim(int _num, float _direction = 1)
    {
        if (_direction > 0) { return aimAnimationData[_num]; }
        else { return aimAnimationData2[_num]; }
    }
    public override RuntimeAnimatorController GetRecoilAnim(int _num, float _direction = 1)
    {
        if (_direction > 0) { return recoilAnimationData[_num]; }
        else { return recoilAnimationData2[_num]; }

    }
}
