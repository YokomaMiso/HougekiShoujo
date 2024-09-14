using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectileBehavior : ProjectileBehavior
{
    [SerializeField] RuntimeAnimatorController projectileUp;
    [SerializeField] RuntimeAnimatorController projectileDown;

    Vector3 defaultPosition;
    Vector3 targetPoint;

    public void ProjectileStart(Vector3 _point)
    {
        targetPoint = _point;
        defaultPosition = this.transform.position;
    }

    protected override void Update()
    {
        float timeRate = timer / lifeTime;

        if (timeRate < 0.5f) { imageAnimator.runtimeAnimatorController = projectileUp; }
        else { imageAnimator.runtimeAnimatorController = projectileDown; }

        base.Update();

        Vector3 currentHorizon = Vector3.Lerp(defaultPosition, targetPoint, timeRate);
        float currentVertical = Mathf.Sin(timeRate * Mathf.PI) * 15;

        transform.position = currentHorizon + Vector3.up * currentVertical;
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }
}