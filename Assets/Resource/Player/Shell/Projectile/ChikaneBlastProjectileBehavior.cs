using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChikaneBlastProjectileBehavior : ProjectileBehavior
{
    int state = 0;
    const int spawnCount = 6;
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
        Vector3 offsetValue = Vector3.right * 2.0f;
        float offsetAngle = 360.0f / spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 offset = Quaternion.Euler(0, offsetAngle * i, 0) * offsetValue;
            Vector3 spawnPos = ownerPlayer.transform.position + offset;
            GameObject explosionInstance = explosion.GetBody();
            GameObject obj = Instantiate(explosionInstance, spawnPos + Vector3.up, Quaternion.identity);
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
