using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileBehavior : ProjectileBehavior
{
    [SerializeField] float speed = 10;

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
}