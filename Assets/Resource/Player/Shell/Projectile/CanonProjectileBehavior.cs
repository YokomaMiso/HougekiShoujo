using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileBehavior : ProjectileBehavior
{
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] float speed = 10;
    float angle = 0;

    protected override void Start()
    {
        base.Start();
        imageAnimator.runtimeAnimatorController = animatorController;
    }

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }

    public void SetAngle(float _angle)
    {
        angle = _angle;
        imageAnimator.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * angle;
    }
}