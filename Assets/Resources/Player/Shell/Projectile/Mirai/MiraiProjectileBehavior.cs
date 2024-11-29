using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraiProjectileBehavior : ProjectileBehavior
{
    protected override void Start()
    {
    }
    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
}
