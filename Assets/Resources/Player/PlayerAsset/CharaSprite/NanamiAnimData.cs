using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Create/SpriteData/CharacterAnim/NanamiData", order = 1)]

public class NanamiAnimData : CharacterAnimData
{
    [SerializeField, Header("Character Idle")] RuntimeAnimatorController idle2AnimationData;
    [SerializeField, Header("Character Run")] RuntimeAnimatorController[] run2AnimationData;

    public override RuntimeAnimatorController GetIdleAnim(CANON_STATE _state = CANON_STATE.EMPTY, float _direction = 1)
    {
        if (_state == CANON_STATE.EMPTY) { return idle2AnimationData; }
        else { return idleAnimationData; }
    }
    public override RuntimeAnimatorController GetRunAnim(int _num, CANON_STATE _state = CANON_STATE.EMPTY, float _direction = 1)
    {
        if (_state == CANON_STATE.EMPTY) { return run2AnimationData[_num]; }
        else { return runAnimationData[_num]; }
    }
}