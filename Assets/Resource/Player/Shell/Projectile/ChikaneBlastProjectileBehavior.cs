using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChikaneBlastProjectileBehavior : ProjectileBehavior
{
    int state = 0;
    const int spawnCount = 3;
    float[] timeBorder = null;

    protected override void Start()
    {
        base.Start();
        timeBorder = new float[4];
        for (int i = 0; i < timeBorder.Length; i++)
        {
            timeBorder[i] = (lifeTime / 4) * (i + 1);
        }
    }

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
        Vector3 offset = transform.right * 1.5f;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = transform.position + offset - (offset * i);
            GameObject explosionInstance = explosion.GetBody();
            GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
            obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
            obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        }
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();

        timer += deltaTime;
        if (timer > lifeTime) { SpawnExplosion(); Destroy(gameObject); }
    }
}
