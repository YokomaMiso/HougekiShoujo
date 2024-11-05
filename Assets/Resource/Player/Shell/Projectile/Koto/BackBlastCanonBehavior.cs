using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastCanonBehavior : ProjectileBehavior
{
    [SerializeField] Explosion backBlastExplosion;
    //const float backBlastTime = 0.25f;
    bool spawnedBackBlast;

    protected override void Start()
    {
        base.Start();
        SpawnBackBlast();
    }

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();

        if (spawnedBackBlast) { return; }
        //if (timer > backBlastTime) { SpawnBackBlast(); }
    }

    void SpawnBackBlast()
    {
        float explosionRadius = backBlastExplosion.GetScale() + 0.25f;
        Vector3 spawnPos = ownerPlayer.transform.position - transform.forward * explosionRadius;

        GameObject obj = Instantiate(backBlastExplosion.GetBody(), spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(backBlastExplosion);
        float angle = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
        obj.transform.GetChild(0).GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * angle;
        spawnedBackBlast = true;
    }
}
