using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class RanProjectileBehaviour : ProjectileBehavior
{

    const int spawnCount = 3;
    int nowCount = 0;
    const float firstSpawn = 0.4f;
    const float spawnInterval = 0.15f;


    Vector3 inputVector;

    protected override void Start()
    {
        base.Start();
        inputVector = ownerPlayer.GetInputVector().normalized;
        if (inputVector == Vector3.zero) { inputVector = Vector3.forward; }
    }

    public override void SetData(Shell _data)
    {
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
    }


    protected override void SpawnExplosion()
    {
        Vector3 forwardVector = inputVector;
        forwardVector.Normalize();
        Vector3 offset = forwardVector * 0.5f;
        Vector3 addPos = offset + forwardVector * explosion.GetScale() * 2 * nowCount;

        Vector3 spawnPos = transform.position + addPos;
        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        nowCount++;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;

        if (timer >= firstSpawn + spawnInterval * nowCount)
        {
            if (nowCount < spawnCount) { SpawnExplosion(); }
        }

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

