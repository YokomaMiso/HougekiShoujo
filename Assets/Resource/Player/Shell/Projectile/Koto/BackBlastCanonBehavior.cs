using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastCanonBehavior : ProjectileBehavior
{
    const float backBlastTime = 0.25f;
    bool spawnedBackBlast;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();

        if (spawnedBackBlast) { return; }
        if (timer > backBlastTime) { SpawnBackBlast(); }
    }

    void SpawnBackBlast()
    {
        float explosionRadius = explosion.GetScale() * 1.5f;
        Vector3 spawnPos = ownerPlayer.transform.position - transform.forward * explosionRadius;

        GameObject obj = Instantiate(explosion.GetBody(), spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        spawnedBackBlast = true;
    }
}
