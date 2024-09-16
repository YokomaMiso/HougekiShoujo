using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    protected int buffNum;

    protected float speedRate = 1.0f;
    protected float lifeTime = 3.0f;

    protected float timer = 0;

    public void SetRateAndTime(float _rate, float _time)
    {
        speedRate = _rate;
        lifeTime = _time;
    }

    void Update()
    {
        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer > lifeTime)
        {
            BuffBehavior();
            Destroy(this);
        }
    }

    protected virtual void BuffBehavior() { }
}
