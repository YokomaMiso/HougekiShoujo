using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Create/SpriteData/CharacterAnim/SaraData", order = 1)]
public class SaraAnimData : CharacterAnimData
{
    [SerializeField, Header("Character Recoil2")] RuntimeAnimatorController recoilAnimationData2;

    public override RuntimeAnimatorController GetRecoilAnim(int _num,float _direction = 1) 
    {
        if (_direction > 0) { return recoilAnimationData[_num]; }
        else {  return recoilAnimationData2; }
    }
}
