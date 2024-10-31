using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraProjectile : ProjectileBehavior
{
    [SerializeField] GameObject fragmentPrefab;

    float addAngle = 0;
    const int spawnCount = 4;

    protected override void Start()
    {
        if (transform.GetChild(0).localScale.x < 0) { addAngle = 180; }
        base.Start();
        imageAnimator.transform.localScale *= 2;
    }

    protected override void Update()
    {
        float timeRate = timer / lifeTime;

        if (timeRate < 0.5f) { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * (90 + addAngle); }
        else { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * -(90 + addAngle); }

        base.Update();

        Vector3 currentHorizon = Vector3.Lerp(defaultPosition, targetPoint, timeRate);
        float currentVertical = Mathf.Sin(timeRate * Mathf.PI) * 15;

        transform.position = currentHorizon + Vector3.up * currentVertical;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        float timeRate = timer / lifeTime;
        if (timeRate < 0.5f) { return; }

        base.OnTriggerEnter(other);
    }

    protected override void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        float timeRate = timer / lifeTime;
        if (timeRate >= 0.95f) { spawnPos.y = 0; }

        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);

        SpawnFragment();
    }

    void SpawnFragment()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 addPos = (targetPoint - defaultPosition).normalized * 3;
            addPos = Quaternion.Euler(0, 45 + 90 * i, 0) * addPos;
            Vector3 spawnPos = transform.position;
            GameObject fragment = Instantiate(fragmentPrefab, spawnPos, Quaternion.identity);
            FragmentBehavior fragmentBehavior = fragment.GetComponent<FragmentBehavior>();
            fragmentBehavior.SetPlayer(ownerPlayer);
            fragmentBehavior.ProjectileStart(spawnPos + addPos);
        }
    }
}
