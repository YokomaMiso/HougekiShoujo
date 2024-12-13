using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHitedPlayer : MonoBehaviour
{
    SubExplosionBehavior parent;
    int num;
    float timer;
    float lifeTime;

    public void SetData(SubExplosionBehavior _parent, int _num, float _lifeTime)
    {
        parent = _parent;
        num = _num;
        lifeTime = _lifeTime;
    }

    void Update()
    {
        if (parent == null)
        {
            Destroy(this);
            return;
        }

        timer += Managers.instance.timeManager.GetDeltaTime();

        if (timer > lifeTime)
        {
            parent.ResetHitedPlayer(num);
            Destroy(this);
        }
    }
}
