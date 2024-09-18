using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastProjectileBehavior : ProjectileBehavior
{
    public override void SetData(Shell _data)
    {
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
        TagSetting();
    }
    protected override void TimeSetting()
    {
    }

    protected override void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        GameObject explosionInstance = explosion.GetBody();
        //spawnPos.y = explosion.GetScale() / 2;
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();

        timer += deltaTime;
        if (timer > lifeTime) { SpawnExplosion(); Destroy(gameObject); }
    }
}
