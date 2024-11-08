using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraiProjectileBehavior : ProjectileBehavior
{
    const float enableTime = 0.25f;

    protected override void Start()
    {
    }
    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (timer < enableTime) { return; }
        base.OnTriggerEnter(other);
    }
}
