using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileBehavior : ProjectileBehavior
{
    [SerializeField] float speed = 10;

    void Start()
    {
    }

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
}