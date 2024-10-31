using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBehavior : ProjectileBehavior
{
    [SerializeField] Shell flagmentData;
    float addAngle = 0;
    const float height = 3;

    protected override void Start()
    {
        SetData(flagmentData);
        if (transform.GetChild(0).localScale.x < 0) { addAngle = 180; }
        base.Start();
    }

    protected override void Update()
    {
        float timeRate = timer / lifeTime;

        if (timeRate < 0.5f) { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * (90 + addAngle); }
        else { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * -(90 + addAngle); }

        //base.Update();
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;

        Vector3 currentHorizon = Vector3.Lerp(defaultPosition, targetPoint, timeRate);
        //float currentVertical = Mathf.Sin(timeRate * Mathf.PI) * 3;
        float currentVertical = -Mathf.Pow(timeRate * 2 - 1.41f, 2) + 2;

        transform.position = currentHorizon + Vector3.up * currentVertical * height;
    }

    protected override void SpawnExplosion()
    {
        Vector3 explosionPos = transform.position;
        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, explosionPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        float timeRate = timer / lifeTime;
        if (timeRate < 0.5f) { return; }

        base.OnTriggerEnter(other);
    }
}

