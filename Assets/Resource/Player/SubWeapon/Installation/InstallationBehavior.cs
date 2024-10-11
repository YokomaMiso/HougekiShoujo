using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationBehavior : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    protected Animator imageAnimator;
    protected float lifeTime;
    protected float timer = 0;
    protected Explosion explosion;

    protected string playerTag = "Player";

    protected bool applyLoop = false;
    protected float changeToLoopAnimTime = 1.5f;
    protected RuntimeAnimatorController startAnim;
    protected RuntimeAnimatorController loopAnim;


    protected virtual void Start()
    {
        lifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime();
        startAnim = ownerPlayer.GetPlayerData().GetSubWeapon().GetInstallationStartAnim();
        loopAnim = ownerPlayer.GetPlayerData().GetSubWeapon().GetInstallationLoopAnim();

        imageAnimator = transform.GetChild(0).GetComponent<Animator>();
        imageAnimator.runtimeAnimatorController = startAnim;
    }
    protected virtual void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }

    public virtual void InstallationAction()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
        {
            InstallationAction();
        }
    }

    protected virtual void TimeSetting()
    {
        if (imageAnimator == null) { return; }
        imageAnimator.speed = 1 * Managers.instance.timeManager.TimeRate();
    }
}
