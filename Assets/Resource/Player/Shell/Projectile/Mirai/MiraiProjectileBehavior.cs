using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraiProjectileBehavior : ProjectileBehavior
{
    public override void SetData(Shell _data)
    {
        imageAnimator = transform.GetChild(0).GetComponent<Animator>();
        imageAnimator.runtimeAnimatorController = _data.GetAnim();
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
        speed = _data.GetSpeed();

        if (_data.GetShellType() != SHELL_TYPE.BLAST) { SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetFlySFX(), transform); }
        transform.position += Quaternion.Euler(0, angle, 0) * transform.forward;

    }
    protected override void Start()
    {
    }
    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
}
