using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastCanonBehavior : ProjectileBehavior
{
    protected override void Start()
    {
        base.Start();

        float explosionRadius = explosion.GetScale() * 2;
        Vector3 spawnPos = transform.position - transform.forward * explosionRadius;

        GameObject obj = Instantiate(explosion.GetBody(), spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
    }

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
}
