using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectileBehavior : ProjectileBehavior
{
    [SerializeField] float speed = 10;
    float angle = 0;

    void Start()
    {
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