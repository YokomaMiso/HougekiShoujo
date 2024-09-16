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

    protected virtual void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }

    protected virtual void InstallationAction()
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
