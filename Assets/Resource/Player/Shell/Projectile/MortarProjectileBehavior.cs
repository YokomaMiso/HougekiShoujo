using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectileBehavior : ProjectileBehavior
{

    protected override void Update()
    {
        float timeRate = timer / lifeTime;

        if (timeRate < 0.5f) { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * 90; }
        else { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * -90; }

        base.Update();

        float horizonTimeRate = timeRate * 2;
        if (horizonTimeRate > 1) { horizonTimeRate = 1; }
        Vector3 currentHorizon = Vector3.Lerp(defaultPosition, targetPoint, timeRate);
        float currentVertical = Mathf.Sin(timeRate * Mathf.PI) * 15;

        transform.position = currentHorizon + Vector3.up * currentVertical;
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }
}